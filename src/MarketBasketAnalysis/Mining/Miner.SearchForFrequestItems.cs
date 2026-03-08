using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Mining
{
    internal sealed partial class Miner
    {
        #region Nested types
        private sealed class SearchForFrequentItemsState
        {
            public IItemExcluder ItemExcluder { get; }

            public IItemConverter ItemConverter { get; }

            public HashSet<Item> Items { get; }

            public Dictionary<Item, int> ItemFrequencies { get; }

            public int TransactionsCount { get; set; }

            public SearchForFrequentItemsState(IItemExcluder itemExcluder, IItemConverter itemConverter)
            {
                ItemExcluder = itemExcluder;
                ItemConverter = itemConverter;
                Items = new HashSet<Item>();
                ItemFrequencies = new Dictionary<Item, int>();
            }

            public void Deconstruct(
                out IItemExcluder itemExcluder,
                out IItemConverter itemConverter,
                out HashSet<Item> items,
                out Dictionary<Item, int> itemFrequencies)
            {
                itemExcluder = ItemExcluder;
                itemConverter = ItemConverter;
                items = Items;
                itemFrequencies = ItemFrequencies;
            }
        }
        #endregion

        #region Methods
        private static SearchForFrequentItemsState ProcessItems(
            IReadOnlyList<Item> transaction,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
            ParallelLoopState _,
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            SearchForFrequentItemsState localState)
        {
            ThrowIfTransactionIsNull(transaction);

            var (itemExcluder, itemConverter, items, itemFrequencies) = localState;

            try
            {
                foreach (var item in transaction)
                {
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

                        UpdateFrequency(itemFrequencies, group);
                    }
                    else
                    {
                        UpdateFrequency(itemFrequencies, item);
                    }
                }

                localState.TransactionsCount++;
            }
            finally
            {
                items.Clear();
            }

            return localState;
        }

        private static void AggregateStates(
            SearchForFrequentItemsState localState,
            ConcurrentDictionary<Item, int> finalItemFrequencies,
            ref int finalTransactionCount,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var pair in localState.ItemFrequencies)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var item = pair.Key;
                var frequency = pair.Value;

                finalItemFrequencies.AddOrUpdate(item, frequency, (_, finalFrequency) => finalFrequency + frequency);
            }

            Interlocked.Add(ref finalTransactionCount, localState.TransactionsCount);
        }

        private static Dictionary<Item, int> SearchForFrequentItems(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            IItemExcluder itemExcluder,
            IItemConverter itemConverter,
            CancellationToken cancellationToken,
            out int transactionCount)
        {
            if ((transactions is ICollection<IReadOnlyList<Item>> collection && collection.Count == 0) ||
                (transactions is IReadOnlyCollection<IReadOnlyList<Item>> readOnlyCollection && readOnlyCollection.Count == 0))
            {
                transactionCount = 0;

                return new Dictionary<Item, int>();
            }

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.DegreeOfParallelism,
            };
            var finalItemFrequencies = new ConcurrentDictionary<Item, int>();
            var finalTransactionsCount = 0;

            Parallel.ForEach(
                transactions,
                parallelOptions,
                () => new SearchForFrequentItemsState(itemExcluder, itemConverter),
                ProcessItems,
                localState => AggregateStates(localState, finalItemFrequencies, ref finalTransactionsCount, cancellationToken));

            transactionCount = finalTransactionsCount;

            var frequencyThreshold = (int)Math.Ceiling(finalTransactionsCount * parameters.MinSupport);

            return finalItemFrequencies
                .Where(keyValuePair => keyValuePair.Value >= frequencyThreshold)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }
        #endregion
    }
}
