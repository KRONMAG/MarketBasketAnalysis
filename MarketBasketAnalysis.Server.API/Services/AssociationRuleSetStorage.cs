using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketBasketAnalysis.Common.Protos;
using MarketBasketAnalysis.Server.API.Extensions;
using MarketBasketAnalysis.Server.Application.Exceptions;
using MarketBasketAnalysis.Server.Application.Services;
using static MarketBasketAnalysis.Common.Protos.AssociationRuleSetPartMessage;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace MarketBasketAnalysis.Server.API.Services;

public class AssociationRuleSetStorage : Common.Protos.AssociationRuleSetStorage.AssociationRuleSetStorageBase
{
    #region Fields and Properties

    private readonly IAssociationRuleSetInfoLoader _associationRuleSetInfoLoader;
    private readonly IAssociationRuleSetLoader _associationRuleSetLoader;
    private readonly IAssociationRuleSetSaver _associationRuleSetSaver;
    private readonly IAssociationRuleSetSaverPool _associationRuleSetSaverPool;
    private readonly IAssociationRuleSetRemover _associationRuleSetRemover;
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public AssociationRuleSetStorage(IAssociationRuleSetInfoLoader associationRuleSetInfoLoader,
        IAssociationRuleSetLoader associationRuleSetLoader, IAssociationRuleSetSaver associationRuleSetSaver,
        IAssociationRuleSetSaverPool associationRuleSetSaverPool,
        IAssociationRuleSetRemover associationRuleSetRemover, ILogger<AssociationRuleSetStorage> logger)
    {
        ArgumentNullException.ThrowIfNull(associationRuleSetInfoLoader);
        ArgumentNullException.ThrowIfNull(associationRuleSetLoader);
        ArgumentNullException.ThrowIfNull(associationRuleSetSaver);
        ArgumentNullException.ThrowIfNull(associationRuleSetSaverPool);
        ArgumentNullException.ThrowIfNull(associationRuleSetRemover);
        ArgumentNullException.ThrowIfNull(logger);

        _associationRuleSetInfoLoader = associationRuleSetInfoLoader;
        _associationRuleSetLoader = associationRuleSetLoader;
        _associationRuleSetSaver = associationRuleSetSaver;
        _associationRuleSetSaverPool = associationRuleSetSaverPool;
        _associationRuleSetRemover = associationRuleSetRemover;
        _logger = logger;
    }

    #endregion

    #region Methods

    #region Get

    public override async Task<GetResponse> Get(Empty request, ServerCallContext context)
    {
        List<AssociationRuleSetInfoMessage>? associationRuleSetInfos = null;

        try
        {
            associationRuleSetInfos = await _associationRuleSetInfoLoader.LoadAsync(context.CancellationToken);
        }
        catch (AssociationRuleSetLoadException e)
        {
            _logger.LogError(e, "Failed to load association rule set info.");

            RpcThrowHelper.Internal(e.Message);
        }

        var response = new GetResponse();

        response.Values.AddRange(associationRuleSetInfos);

        return response;
    }

    #endregion

    #region Load

    public override async Task Load(LoadRequest request, IServerStreamWriter<LoadResponse> responseStream,
        ServerCallContext context)
    {
        try
        {
            var associationRuleSetInfo = await _associationRuleSetLoader.LoadAssociationRuleSetInfoAsync(
                request.AssociationRuleSetName, context.CancellationToken);

            await responseStream.WriteAsync(
                new LoadResponse
                {
                    AssociationRuleSetPart = new AssociationRuleSetPartMessage
                    {
                        AssociationRuleSetInfo = associationRuleSetInfo
                    }
                }, context.CancellationToken);

            var itemChunks =
                _associationRuleSetLoader.LoadItemChunksAsync(request.AssociationRuleSetName,
                    context.CancellationToken);

            await foreach (var itemChunk in itemChunks)
            {
                await responseStream.WriteAsync(
                    new LoadResponse
                    {
                        AssociationRuleSetPart = new AssociationRuleSetPartMessage { ItemChunk = itemChunk }
                    }, context.CancellationToken);
            }

            var associationRuleChunks = _associationRuleSetLoader
                .LoadAssociationRuleChunksAsync(request.AssociationRuleSetName, context.CancellationToken);

            await foreach (var associationRuleChunk in associationRuleChunks)
            {
                await responseStream.WriteAsync(
                    new LoadResponse
                    {
                        AssociationRuleSetPart = new AssociationRuleSetPartMessage
                        {
                            AssociationRuleChunk = associationRuleChunk
                        }
                    }, context.CancellationToken);
            }
        }
        catch (AssociationRuleSetValidationException e)
        {
            RpcThrowHelper.InvalidArgument(e.Message);
        }
        catch (AssociationRuleSetNotFoundException e)
        {
            RpcThrowHelper.NotFound(e.Message);
        }
        catch (AssociationRuleSetLoadException e)
        {
            _logger.LogError(e, "Failed to load association rule set.");

            RpcThrowHelper.Internal(e.Message);
        }
    }

    #endregion

    #region Save

    public override async Task<Empty> Save(IAsyncStreamReader<SaveRequest> requestStream, ServerCallContext context)
    {
        try
        {
            while (await requestStream.MoveNext(context.CancellationToken))
            {
                var part = requestStream.Current.AssociationRuleSetPart;

                if (part == null)
                {
                    _ = _associationRuleSetSaver.RollbackChangesAsync();

                    RpcThrowHelper.InvalidArgument("Association rule part type not specified.");
                }

                switch (part.PartTypeCase)
                {
                    case PartTypeOneofCase.AssociationRuleSetInfo:
                        await _associationRuleSetSaver.SaveAssociationRuleSetInfoAsync(part.AssociationRuleSetInfo,
                            context.CancellationToken);
                        break;

                    case PartTypeOneofCase.ItemChunk:
                        await _associationRuleSetSaver.SaveItemChunk(part.ItemChunk, context.CancellationToken);
                        break;

                    case PartTypeOneofCase.AssociationRuleChunk:
                        await _associationRuleSetSaver.SaveAssociationRuleChunk(part.AssociationRuleChunk,
                            context.CancellationToken);
                        break;
                }
            }

            await _associationRuleSetSaver.MarkSetAsAvailableAsync(context.CancellationToken);
        }
        catch (AssociationRuleSetValidationException e)
        {
            _logger.LogInformation(e, "Validation error occured while loading association rule set.");

            _ = _associationRuleSetSaver.RollbackChangesAsync();

            RpcThrowHelper.InvalidArgument(e.Message);
        }
        catch (AssociationRuleSetSaveException e)
        {
            _logger.LogError(e, "Failed to save association rule set.");

            _ = _associationRuleSetSaver.RollbackChangesAsync();

            RpcThrowHelper.Internal(e.Message);
        }
        catch (OperationCanceledException)
        {
            _ = _associationRuleSetSaver.RollbackChangesAsync();

            throw;
        }

        return new();
    }

    public override async Task<Empty> SavePart(SavePartRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.TransactionId, out _))
            RpcThrowHelper.InvalidArgument("Transaction ID should be string in GUID format.");

        if (request.AssociationRuleSetPart == null)
            RpcThrowHelper.InvalidArgument("Association rule part type not specified.");

        var associationRuleSetSaver = await _associationRuleSetSaverPool.TryRentAsync(request.TransactionId);

        if (associationRuleSetSaver == null)
        {
            RpcThrowHelper.FailedPrecondition(
                "Requests to save parts of same association rule set must be called sequentially.");
        }

        try
        {
            var part = request.AssociationRuleSetPart;

            switch (part.PartTypeCase)
            {
                case PartTypeOneofCase.AssociationRuleSetInfo:
                    await associationRuleSetSaver.SaveAssociationRuleSetInfoAsync(part.AssociationRuleSetInfo,
                        context.CancellationToken);
                    break;

                case PartTypeOneofCase.ItemChunk:
                    await associationRuleSetSaver.SaveItemChunk(part.ItemChunk, context.CancellationToken);
                    break;

                case PartTypeOneofCase.AssociationRuleChunk:
                    await associationRuleSetSaver.SaveAssociationRuleChunk(part.AssociationRuleChunk,
                        context.CancellationToken);
                    break;
            }

            if (request.IsLastPart)
            {
                await associationRuleSetSaver.MarkSetAsAvailableAsync(context.CancellationToken);
                await _associationRuleSetSaverPool.RemoveAsync(request.TransactionId);
            }
        }
        catch (AssociationRuleSetValidationException e)
        {
            _logger.LogInformation(e, "Validation error occured while loading association rule set.");

            await HandleFailureAsync();

            RpcThrowHelper.InvalidArgument(e.Message);
        }
        catch (AssociationRuleSetSaveException e)
        {
            _logger.LogError(e, "Failed to save association rule set.");

            await HandleFailureAsync();

            RpcThrowHelper.Internal(e.Message);
        }
        catch (OperationCanceledException)
        {
            await HandleFailureAsync();

            throw;
        }
        finally
        {
            await _associationRuleSetSaverPool.ReturnAsync(request.TransactionId);
        }

        return new();

        async Task HandleFailureAsync()
        {
            _ = associationRuleSetSaver.RollbackChangesAsync();
            await _associationRuleSetSaverPool.RemoveAsync(request.TransactionId);
        }
    }

    #endregion

    #region Remove

    public override async Task<Empty> Remove(RemoveRequest request, ServerCallContext context)
    {
        try
        {
            await _associationRuleSetRemover.RemoveAsync(request.AssociationRuleSetName, context.CancellationToken);
        }
        catch (AssociationRuleSetValidationException e)
        {
            RpcThrowHelper.InvalidArgument(e.Message);
        }
        catch (AssociationRuleSetNotFoundException e)
        {
            RpcThrowHelper.NotFound(e.Message);
        }
        catch (AssociationRuleSetRemoveException e)
        {
            _logger.LogError(e, "Failed to remove association rule set.");

            RpcThrowHelper.Internal(e.Message);
        }

        return new();
    }

    #endregion

    #endregion
}