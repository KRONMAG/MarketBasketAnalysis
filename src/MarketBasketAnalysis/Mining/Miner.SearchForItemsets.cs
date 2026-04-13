using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.ObjectPool;
using Timer = System.Timers.Timer;

namespace MarketBasketAnalysis.Mining
{
    internal sealed partial class Miner
    {
        #region Nested types
        private sealed class ItemsetsPoolPolicy : IPooledObjectPolicy<HashSet<(Item, Item)>>
        {
            public HashSet<(Item, Item)> Create() => new HashSet<(Item, Item)>();

            public bool Return(HashSet<(Item, Item)> itemsets)
            {
                itemsets.Clear();

                return true;
            }
        }

        private sealed class SearchForItemsetsState
        {
            private int _processedTransactionsCount;

            public IItemConverter ItemConverter { get; }

            public Dictionary<Item, int> ItemFrequencies { get; }

            public ObjectPool<HashSet<(Item, Item)>> ItemsetsPool { get; }

            public ConcurrentDictionary<(Item, Item), int> ItemsetFrequencies { get; }

            public int ProcessedTransactionsCount => _processedTransactionsCount;

            public SearchForItemsetsState(
                MiningParameters parameters,
                IItemConverter itemConverter,
                Dictionary<Item, int> itemFrequencies,
                ObjectPool<HashSet<(Item, Item)>> itemsetsPool)
            {
                ItemConverter = itemConverter;
                ItemFrequencies = itemFrequencies;
                ItemsetsPool = itemsetsPool;
                ItemsetFrequencies = new ConcurrentDictionary<(Item, Item), int>(parameters.DegreeOfParallelism, 0);
            }

            public void IncrementProcessedTransactionsCount() =>
                Interlocked.Increment(ref _processedTransactionsCount);

            public void Deconstruct(
                out IItemConverter itemConverter,
                out ObjectPool<HashSet<(Item, Item)>> itemsetsPool,
                out Dictionary<Item, int> itemFrequencies,
                out ConcurrentDictionary<(Item, Item), int> itemsetFrequencies)
            {
                itemConverter = ItemConverter;
                itemsetsPool = ItemsetsPool;
                itemFrequencies = ItemFrequencies;
                itemsetFrequencies = ItemsetFrequencies;
            }
        }

        private sealed class SearchForItemsetsStateProvider
        {
            private readonly MiningParameters _parameters;
            private readonly IItemConverter _itemConverter;
            private readonly Dictionary<Item, int> _itemFrequencies;
            private readonly ObjectPool<HashSet<(Item, Item)>> _itemsetsPool;
            private readonly ConcurrentDictionary<int, SearchForItemsetsState> _states;
            private int _counter;

            public SearchForItemsetsStateProvider(
                MiningParameters parameters,
                IItemConverter itemConverter,
                Dictionary<Item, int> itemFrequencies)
            {
                _parameters = parameters;
                _itemConverter = itemConverter;
                _itemFrequencies = itemFrequencies;
                _itemsetsPool = new DefaultObjectPool<HashSet<(Item, Item)>>(
                    new ItemsetsPoolPolicy(),
                    parameters.DegreeOfParallelism);
                _states = new ConcurrentDictionary<int, SearchForItemsetsState>();
            }

            public SearchForItemsetsState GetOrCreateState()
            {
                var key = Interlocked.Increment(ref _counter) % _parameters.StatePartitionCount;

                return _states.GetOrAdd(key, ValueFactory);

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                SearchForItemsetsState ValueFactory(int _) =>
                    new SearchForItemsetsState(_parameters, _itemConverter, _itemFrequencies, _itemsetsPool);
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            }

            public void AggregateStates(out IReadOnlyDictionary<(Item, Item), int> itemsetFrequencies)
            {
                if (_states.Count == 1)
                {
                    var state = _states.First().Value;

                    itemsetFrequencies = state.ItemsetFrequencies;

                    return;
                }

                var itemsetFrequenciesImpl = new Dictionary<(Item, Item), int>();

                foreach (var state in _states.Values)
                {
                    foreach (var pair in state.ItemsetFrequencies)
                    {
                        var (itemset, itemsetFrequency) = (pair.Key, pair.Value);

                        if (!itemsetFrequenciesImpl.ContainsKey(itemset))
                        {
                            itemsetFrequenciesImpl.Add(itemset, itemsetFrequency);
                        }
                        else
                        {
                            itemsetFrequenciesImpl[itemset] += itemsetFrequency;
                        }
                    }
                }

                itemsetFrequencies = itemsetFrequenciesImpl;
            }

            public int GetProcessedTransactionsCount() =>
                _states.Values.Sum(s => s.ProcessedTransactionsCount);
        }
        #endregion

        #region Methods
        private static SearchForItemsetsState ProcessTransaction(
            IReadOnlyList<Item> transaction,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
            ParallelLoopState _,
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            SearchForItemsetsState state)
        {
            ThrowIfTransactionIsNull(transaction);

            var (itemConverter, itemsetsPool, itemFrequencies, itemsetFrequencies) = state;
            var itemsets = itemsetsPool.Get();

            try
            {
                for (var i = 0; i < transaction.Count; i++)
                {
                    ThrowIfItemIsNull(transaction[i]);

                    for (var j = i + 1; j < transaction.Count; j++)
                    {
                        ThrowIfItemIsNull(transaction[j]);

                        var itemset = transaction[i].Id < transaction[j].Id
                            ? (transaction[i], transaction[j])
                            : (transaction[j], transaction[i]);

                        if (itemsets.Contains(itemset) || itemset.Item1.Equals(itemset.Item2))
                        {
                            continue;
                        }

                        itemsets.Add(itemset);

                        if (itemConverter != null)
                        {
                            var isItem1Converted = itemConverter.TryConvert(itemset.Item1, out var item1Group);
                            var isItem2Converted = itemConverter.TryConvert(itemset.Item2, out var item2Group);

                            if (isItem1Converted)
                            {
                                itemset.Item1 = item1Group;
                            }

                            if (isItem2Converted)
                            {
                                itemset.Item2 = item2Group;
                            }

                            var shouldSkipItemset = (isItem1Converted || isItem2Converted) &&
                                                    (itemsets.Contains(itemset) || itemset.Item1.Equals(itemset.Item2));

                            if (shouldSkipItemset)
                            {
                                continue;
                            }

                            itemsets.Add(itemset);
                        }

                        if (itemFrequencies.ContainsKey(itemset.Item1) && itemFrequencies.ContainsKey(itemset.Item2))
                        {
                            itemsetFrequencies.AddOrUpdate(itemset, 1, UpdateFrequency);
                        }
                    }
                }

                state.IncrementProcessedTransactionsCount();

                return state;
            }
            finally
            {
                itemsetsPool.Return(itemsets);
            }
        }

        private IReadOnlyDictionary<(Item, Item), int> SearchForItemsets(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            IItemConverter itemConverter,
            Dictionary<Item, int> itemFrequencies,
            int transactionCount,
            CancellationToken cancellationToken)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (IsTransactionsEmptyCollection(transactions))
            {
                return new ConcurrentDictionary<(Item, Item), int>();
            }

            var stateProvider = new SearchForItemsetsStateProvider(parameters, itemConverter, itemFrequencies);
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.DegreeOfParallelism,
            };

            var timer = new Timer(parameters.MiningProgressInterval);

            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            try
            {
                // ReSharper disable once PossibleMultipleEnumeration
                Parallel.ForEach(
                    transactions,
                    parallelOptions,
                    stateProvider.GetOrCreateState,
                    ProcessTransaction,
                    _ => { });
            }
            finally
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Dispose();
            }

            stateProvider.AggregateStates(out var itemsetFrequencies);

            return itemsetFrequencies;

            // ToDo: calculate progress value more accurately
            // ReSharper disable once InconsistentNaming
            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                var progress = stateProvider.GetProcessedTransactionsCount() / (double)transactionCount * 100;

                OnMiningProgressChanged(progress);
            }
        }
        #endregion
    }
}