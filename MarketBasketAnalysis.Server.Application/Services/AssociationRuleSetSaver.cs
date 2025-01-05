using Google.Protobuf;
using MarketBasketAnalysis.Common.Protos;
using MarketBasketAnalysis.Server.Application.Exceptions;
using MarketBasketAnalysis.Server.Application.Extensions;
using MarketBasketAnalysis.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System.Buffers;

namespace MarketBasketAnalysis.Server.Application.Services;

public sealed class AssociationRuleSetSaver : IAssociationRuleSetSaver
{
    #region Fields and Properties

    private const int RollbackChangesRetryCount = 5;
    private const int RollbackChangesInitialBackoff = 2;
    private const double RollbackChangesBackoffMultiplier = 1.5;

    private readonly MarketBasketAnalysisDbContext _context;
    private readonly ILogger<AssociationRuleSetSaver> _logger;

    private AssociationRuleSet? _associationRuleSet;
    private Dictionary<int, int>? _itemCounts;

    #endregion

    #region Constructors

    public AssociationRuleSetSaver(MarketBasketAnalysisDbContext context, ILogger<AssociationRuleSetSaver> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    #endregion

    #region Methods

    #region Save

    public async Task SaveAssociationRuleSetInfoAsync(AssociationRuleSetInfoMessage message,
        CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        try
        {
            if (_associationRuleSet != null)
                throw new AssociationRuleSetSaveException("Association rule set info already saved.");

            await ValidateAssociationRuleSetInfoAsync(message, token);

            _associationRuleSet = new AssociationRuleSet()
            {
                Name = message.Name,
                Description = message.Description,
                TransactionCount = message.TransactionCount
            };
            _itemCounts = new Dictionary<int, int>();

            await _context.AddAsync(_associationRuleSet, token);
            await _context.SaveChangesAsync(token);
        }
        catch (DbUpdateException e)
        {
            _associationRuleSet = null;
            _itemCounts = null;

            throw new AssociationRuleSetSaveException(
                "Unexpected error occured while saving association rule set info.", e);
        }
        finally
        {
            if (_associationRuleSet != null)
                _context.Entry(_associationRuleSet).State = EntityState.Detached;
        }
    }

    public async Task SaveItemChunk(ItemChunkMessage message, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (_associationRuleSet == null)
            throw new AssociationRuleSetSaveException("Item chunks should be saved after association rule set info.");

        ValidateItemChunk(message);

        await SaveChunkMessageAsync(message,
            (data, payloadSize) => new ItemChunk
            {
                Data = data,
                PayloadSize = payloadSize,
                AssociationRuleSetId = _associationRuleSet.Id
            }, token);
    }

    public async Task SaveAssociationRuleChunk(AssociationRuleChunkMessage message, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (_associationRuleSet == null || _itemCounts?.Count == 0)
        {
            throw new AssociationRuleSetSaveException(
                "Association rule chunks should be saved after association rule set info and item chunks.");
        }

        ValidateAssociationRuleChunk(message);

        await SaveChunkMessageAsync(message,
            (data, payloadSize) => new AssociationRuleChunk
            {
                Data = data,
                PayloadSize = payloadSize,
                AssociationRuleSetId = _associationRuleSet.Id
            }, token);
    }

    private async Task SaveChunkMessageAsync<TEntity, TMessage>(TMessage message,
        Func<byte[], int, TEntity> entityFactory, CancellationToken token)
        where TEntity : notnull
        where TMessage : IMessage
    {
        var messageSize = message.CalculateSize();
        var buffer = ArrayPool<byte>.Shared.Rent(messageSize);
        var bufferSegment = new ArraySegment<byte>(buffer, 0, messageSize);

        message.WriteTo(bufferSegment);

        var entity = entityFactory(buffer, messageSize);

        _context.Add(entity);

        try
        {
            await _context.SaveChangesAsync(token);
        }
        catch (Exception e) when (e.IsDbOrDbUpdateException())
        {
            var chunkMessageName = message is ItemChunkMessage ? "item" : "association rule";
            var errorMessage = $"Unexpected error occured while saving {chunkMessageName} chunk.";

            throw new AssociationRuleSetSaveException(errorMessage, e);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            _context.Entry(entity).State = EntityState.Detached;
        }
    }

    #endregion

    #region Commit/Rollback Changes

    public async Task MarkSetAsAvailableAsync(CancellationToken token = default)
    {
        if (_associationRuleSet == null)
            return;

        _associationRuleSet.IsAvailable = true;

        _context.Update(_associationRuleSet);

        try
        {
            await _context.SaveChangesAsync(token);
        }
        catch (Exception e) when (e.IsDbOrDbUpdateException())
        {
            throw new AssociationRuleSetSaveException(
                "Unexpected error occured while marking association rule set as available.");
        }
        finally
        {
            _context.Entry(_associationRuleSet).State = EntityState.Detached;
        }

        _associationRuleSet = null;
        _itemCounts = null;
    }

    public async Task RollbackChangesAsync(CancellationToken token = default)
    {
        if (_associationRuleSet == null)
            return;

        _context.Remove(_associationRuleSet);

        try
        {
            await Policy
                .Handle<Exception>(e => e.IsDbOrDbUpdateException())
                .WaitAndRetryAsync(RollbackChangesRetryCount, n =>
                {
                    var durationInSeconds =
                        RollbackChangesInitialBackoff * Math.Pow(RollbackChangesBackoffMultiplier, n - 1);

                    return TimeSpan.FromSeconds(durationInSeconds);
                })
                .ExecuteAsync(async () => await _context.SaveChangesAsync(token));
        }
        catch (Exception e) when (e.IsDbOrDbUpdateException())
        {
            _logger.LogError(e, "Failed to rollback changes made while saving association rule set.");
        }
        finally
        {
            _context.Entry(_associationRuleSet).State = EntityState.Detached;
            _associationRuleSet = null;
            _itemCounts = null;
        }
    }

    #endregion

    #region Validate

    private async Task ValidateAssociationRuleSetInfoAsync(AssociationRuleSetInfoMessage message, CancellationToken token)
    {
        message.Name.ValidateAssociationRuleSetName();

        if (message.TransactionCount <= 0)
        {
            throw new AssociationRuleSetValidationException(
                "Association rule set should have positive transaction count.");
        }

        var isAlreadyExists = await _context.AssociationRuleSets.AnyAsync(e => e.Name == message.Name, token);

        if (isAlreadyExists)
        {
            throw new AssociationRuleSetValidationException(
                $"Association rule set with name \"{message.Name}\" already exists.");
        }
    }

    private void ValidateItemChunk(ItemChunkMessage message)
    {
        foreach (var itemMessage in message.Values)
        {
            if (itemMessage.Name == null)
            {
                throw new AssociationRuleSetValidationException(
                    $"Item with ID {itemMessage.Id} should have non-null name.");
            }

            if (itemMessage.Count <= 0)
            {
                throw new AssociationRuleSetValidationException(
                    $"Count of item with ID {itemMessage.Id} should be positive.");
            }

            if (_itemCounts!.ContainsKey(itemMessage.Id))
                throw new AssociationRuleSetValidationException($"Item with ID {itemMessage.Id} is duplicated.");

            if (itemMessage.Count > _associationRuleSet!.TransactionCount)
            {
                throw new AssociationRuleSetValidationException(
                    $"Count of item  with ID {itemMessage.Id} should be less than or equal to transaction count.");
            }

            _itemCounts!.Add(itemMessage.Id, itemMessage.Count);
        }
    }

    private void ValidateAssociationRuleChunk(AssociationRuleChunkMessage message)
    {
        foreach (var itemMessage in message.Values)
        {
            var leftHandSideId = itemMessage.LeftHandSideId;
            var rightHandSideId = itemMessage.RightHandSideId;
            var handSidesCount = itemMessage.Count;

            if (leftHandSideId == rightHandSideId)
            {
                throw new AssociationRuleSetValidationException(
                    $"Left and right hand side IDs should be different, but they are equal to {leftHandSideId}.");
            }

            if (itemMessage.Count <= 0)
            {
                throw new AssociationRuleSetValidationException(
                    $"Count of association rule with hand side IDs {leftHandSideId} and {rightHandSideId} should be positive.");
            }

            if (!_itemCounts!.TryGetValue(leftHandSideId, out var leftHandSideCount))
            {
                throw new AssociationRuleSetValidationException(
                    $"Left hand side with ID {leftHandSideId} not found in items list.");
            }

            if (!_itemCounts!.TryGetValue(rightHandSideId, out var rightHandSideCount))
            {
                throw new AssociationRuleSetValidationException(
                    $"Right hand side with ID {rightHandSideId} not found in items list.");
            }

            if (handSidesCount > leftHandSideCount)
            {
                throw new AssociationRuleSetValidationException(
                    $"Count of association rule with hand side IDs {leftHandSideId} " +
                    $"and {rightHandSideId} should be less than or equal to left hand side count.");
            }

            if (handSidesCount > rightHandSideCount)
            {
                throw new AssociationRuleSetValidationException(
                    $"Count of association rule with hand side IDs {leftHandSideId} " +
                    $"and {rightHandSideId} should be less than or equal to right hand side count.");
            }
        }
    }

    #endregion

    #endregion
}