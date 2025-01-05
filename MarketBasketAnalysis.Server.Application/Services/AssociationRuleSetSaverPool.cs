using MarketBasketAnalysis.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace MarketBasketAnalysis.Server.Application.Services;

public sealed class AssociationRuleSetSaverPool : IAssociationRuleSetSaverPool
{
    #region Nested Types
    private sealed record EntryValue(
        AssociationRuleSetSaver AssociationRuleSetSaver,
        MarketBasketAnalysisDbContext Context)
    {
        public bool IsInUse { get; set; }
    }

    #endregion

    #region Fields and Properties

    private const int SlidingExpirationInMinutes = 5;

    private readonly IDbContextFactory<MarketBasketAnalysisDbContext> _contextFactory;
    private readonly ILoggerFactory _loggerFactory;

    private readonly MemoryCache _cache;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _syncObjects;

    private bool _disposed;

    #endregion

    #region Constructors

    public AssociationRuleSetSaverPool(
        IDbContextFactory<MarketBasketAnalysisDbContext> contextFactory,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(contextFactory);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _contextFactory = contextFactory;
        _loggerFactory = loggerFactory;

        _cache = new(Options.Create(new MemoryCacheOptions()));
        _syncObjects = new();
    }

    #endregion

    #region Methods

    public async Task<IAssociationRuleSetSaver?> TryRentAsync(string key)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);

        var syncObject = _syncObjects.GetOrAdd(key, _ => new(1));

        await syncObject.WaitAsync();

        try
        {
            var value = await _cache.GetOrCreateAsync(key, async entry =>
            {
                var context = await _contextFactory.CreateDbContextAsync();
                var logger = _loggerFactory.CreateLogger<AssociationRuleSetSaver>();
                var associationRuleSetSaver = new AssociationRuleSetSaver(context, logger);
                var value = new EntryValue(associationRuleSetSaver, context);

                entry.Value = value;
                entry.SlidingExpiration = TimeSpan.FromSeconds(SlidingExpirationInMinutes);
                entry.PostEvictionCallbacks.Add(new()
                {
                    // ReSharper disable once AsyncVoidLambda
                    EvictionCallback = async (_, valueObj, reason, _) =>
                    {
                        // ReSharper disable once VariableHidesOuterVariable
                        var value = (EntryValue)valueObj!;

                        if (reason == EvictionReason.Expired)
                            // ReSharper disable once MethodSupportsCancellation
                            await value.AssociationRuleSetSaver.RollbackChangesAsync();

                        await value.Context.DisposeAsync();

                        // ReSharper disable once VariableHidesOuterVariable
                        if (_syncObjects.TryRemove(key, out var syncObject))
                            syncObject.Dispose();
                    }
                });

                return value;
            });

            return value!.IsInUse ? null : value.AssociationRuleSetSaver;
        }
        finally
        {
            syncObject.Release();
        }
    }

    public async Task ReturnAsync(string key)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);

        if (!_syncObjects.TryGetValue(key, out var syncObject))
            return;

        await syncObject.WaitAsync();

        try
        {
            if (!_cache.TryGetValue<EntryValue>(key, out var value))
                return;

            value!.IsInUse = false;
        }
        finally
        {
            syncObject.Release();
        }
    }

    public async Task RemoveAsync(string key)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);

        if (!_syncObjects.TryGetValue(key, out var syncObject))
            return;

        await syncObject.WaitAsync();

        try
        {
            _cache.Remove(key);
        }
        finally
        {
            syncObject.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _cache.Clear();
        _cache.Dispose();

        _disposed = true;
    }

    #endregion
}