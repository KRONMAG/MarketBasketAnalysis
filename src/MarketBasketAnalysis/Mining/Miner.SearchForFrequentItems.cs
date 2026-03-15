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
            private int _transactionsCount;

            public IItemExcluder ItemExcluder { get; }

            public IItemConverter ItemConverter { get; }

            public ObjectPool<HashSet<Item>> ItemsPool { get; }

            public ConcurrentDictionary<Item, int> ItemFrequencies { get; }

            public int TransactionsCount => _transactionsCount;

            public SearchForFrequentItemsState(
                IItemExcluder itemExcluder,
                IItemConverter itemConverter,
                MiningParameters parameters)
            {
                ItemExcluder = itemExcluder;
                ItemConverter = itemConverter;
                ItemsPool = new DefaultObjectPool<HashSet<Item>>(new ItemsPoolPolicy(), parameters.DegreeOfParallelism);
                ItemFrequencies = new ConcurrentDictionary<Item, int>(parameters.DegreeOfParallelism, 0);
            }

            public void IncrementTransactionsCount() =>
                Interlocked.Increment(ref _transactionsCount);

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

            var state = new SearchForFrequentItemsState(itemExcluder, itemConverter, parameters);
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.DegreeOfParallelism,
            };

            // ReSharper disable once PossibleMultipleEnumeration
            Parallel.ForEach(transactions, parallelOptions, () => state, ProcessTransaction, _ => { });

            transactionCount = state.TransactionsCount;

            var frequencyThreshold = (int)Math.Ceiling(transactionCount * parameters.MinSupport);

            return state.ItemFrequencies
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

                state.IncrementTransactionsCount();

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
