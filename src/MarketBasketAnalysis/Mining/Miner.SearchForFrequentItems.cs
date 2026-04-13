using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;

namespace MarketBasketAnalysis.Mining
{
    internal sealed partial class Miner
    {
        #region Nested types
        private sealed class ItemsPoolPolicy : IPooledObjectPolicy<HashSet<Item>>
        {
            public HashSet<Item> Create() => new HashSet<Item>();

            public bool Return(HashSet<Item> items)
            {
                items.Clear();

                return true;
            }
        }

        private sealed class SearchForFrequentItemsState
        {
            private int _processedTransactionsCount;

            public IItemExcluder ItemExcluder { get; }

            public IItemConverter ItemConverter { get; }

            public ObjectPool<HashSet<Item>> ItemsPool { get; }

            public ConcurrentDictionary<Item, int> ItemFrequencies { get; }

            public int ProcessedTransactionsCount => _processedTransactionsCount;

            public SearchForFrequentItemsState(
                MiningParameters parameters,
                IItemExcluder itemExcluder,
                IItemConverter itemConverter,
                ObjectPool<HashSet<Item>> itemsPool)
            {
                ItemExcluder = itemExcluder;
                ItemConverter = itemConverter;
                ItemsPool = itemsPool;
                ItemFrequencies = new ConcurrentDictionary<Item, int>(parameters.DegreeOfParallelism, 0);
            }

            public void IncrementProcessedTransactionsCount() =>
                Interlocked.Increment(ref _processedTransactionsCount);

            public void Deconstruct(
                out IItemExcluder itemExcluder,
                out IItemConverter itemConverter,
                out ObjectPool<HashSet<Item>> itemsPool,
                out ConcurrentDictionary<Item, int> itemFrequencies)
            {
                itemExcluder = ItemExcluder;
                itemConverter = ItemConverter;
                itemsPool = ItemsPool;
                itemFrequencies = ItemFrequencies;
            }
        }

        private sealed class SearchForFrequentItemsStateProvider
        {
            private readonly MiningParameters _parameters;
            private readonly IItemExcluder _itemExcluder;
            private readonly IItemConverter _itemConverter;
            private readonly ObjectPool<HashSet<Item>> _itemsPool;
            private readonly ConcurrentDictionary<int, SearchForFrequentItemsState> _states;
            private int _counter;

            public SearchForFrequentItemsStateProvider(
                MiningParameters parameters,
                IItemExcluder itemExcluder,
                IItemConverter itemConverter)
            {
                _parameters = parameters;
                _itemExcluder = itemExcluder;
                _itemConverter = itemConverter;
                _itemsPool = new DefaultObjectPool<HashSet<Item>>(
                    new ItemsPoolPolicy(),
                    parameters.DegreeOfParallelism);
                _states = new ConcurrentDictionary<int, SearchForFrequentItemsState>();
            }

            public SearchForFrequentItemsState GetOrCreateState()
            {
                var key = Interlocked.Increment(ref _counter) % _parameters.StatePartitionCount;

                return _states.GetOrAdd(key, ValueFactory);

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                SearchForFrequentItemsState ValueFactory(int _) =>
                    new SearchForFrequentItemsState(_parameters, _itemExcluder, _itemConverter, _itemsPool);
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            }

            public void AggregateStates(
                out IReadOnlyDictionary<Item, int> itemFrequencies,
                out int transactionsCount)
            {
                if (_states.Count == 1)
                {
                    var state = _states.First().Value;

                    itemFrequencies = state.ItemFrequencies;
                    transactionsCount = state.ProcessedTransactionsCount;

                    return;
                }

                var itemFrequenciesImpl = new Dictionary<Item, int>();

                foreach (var state in _states.Values)
                {
                    foreach (var pair in state.ItemFrequencies)
                    {
                        var (item, itemFrequency) = (pair.Key, pair.Value);

                        if (!itemFrequenciesImpl.ContainsKey(item))
                        {
                            itemFrequenciesImpl.Add(item, itemFrequency);
                        }
                        else
                        {
                            itemFrequenciesImpl[item] += itemFrequency;
                        }
                    }
                }

                itemFrequencies = itemFrequenciesImpl;
                transactionsCount = _states.Values.Sum(i => i.ProcessedTransactionsCount);
            }
        }
        #endregion

        #region Methods
        private static Dictionary<Item, int> SearchForFrequentItems(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            IItemExcluder itemExcluder,
            IItemConverter itemConverter,
            CancellationToken cancellationToken,
            out int transactionCount)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (IsTransactionsEmptyCollection(transactions))
            {
                transactionCount = 0;

                return new Dictionary<Item, int>();
            }

            var stateProvider = new SearchForFrequentItemsStateProvider(parameters, itemExcluder, itemConverter);
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.DegreeOfParallelism,
            };

            // ReSharper disable once PossibleMultipleEnumeration
            Parallel.ForEach(
                transactions,
                parallelOptions,
                stateProvider.GetOrCreateState,
                ProcessTransaction,
                _ => { });

            stateProvider.AggregateStates(out var itemFrequencies, out transactionCount);

            var frequencyThreshold = (int)Math.Ceiling(transactionCount * parameters.MinSupport);

            return itemFrequencies
                .Where(keyValuePair => keyValuePair.Value >= frequencyThreshold)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        private static SearchForFrequentItemsState ProcessTransaction(
            IReadOnlyList<Item> transaction,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
            ParallelLoopState _,
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            SearchForFrequentItemsState state)
        {
            ThrowIfTransactionIsNull(transaction);

            var (itemExcluder, itemConverter, itemsPool, itemFrequencies) = state;
            var items = itemsPool.Get();

            try
            {
                foreach (var item in transaction)
                {
                    ThrowIfItemIsNull(item);

                    if (items.Contains(item) || itemExcluder?.ShouldExclude(item) == true)
                    {
                        continue;
                    }

                    items.Add(item);

                    if (itemConverter?.TryConvert(item, out var group) == true)
                    {
                        if (items.Contains(group) || itemExcluder?.ShouldExclude(group) == true)
                        {
                            continue;
                        }

                        items.Add(group);

                        itemFrequencies.AddOrUpdate(group, 1, UpdateFrequency);
                    }
                    else
                    {
                        itemFrequencies.AddOrUpdate(item, 1, UpdateFrequency);
                    }
                }

                state.IncrementProcessedTransactionsCount();

                return state;
            }
            finally
            {
                itemsPool.Return(items);
            }
        }
        #endregion
    }
}
