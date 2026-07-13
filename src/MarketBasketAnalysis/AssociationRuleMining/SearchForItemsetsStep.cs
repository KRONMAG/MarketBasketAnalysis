using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.AssociationRuleMining.Extensions;
using MarketBasketAnalysis.Extensions;
using MarketBasketAnalysis.Models;
using Microsoft.Extensions.ObjectPool;
using Timer = System.Timers.Timer;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class SearchForItemsetsStep : ISearchForItemsetsStep
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

            public IReadOnlyDictionary<Item, int> FrequentItems { get; }

            public ObjectPool<HashSet<(Item, Item)>> ItemsetsPool { get; }

            public ConcurrentDictionary<(Item, Item), int> ItemsetFrequencies { get; }

            public int ProcessedTransactionsCount => _processedTransactionsCount;

            public SearchForItemsetsState(
                MiningParameters parameters,
                IItemConverter itemConverter,
                IReadOnlyDictionary<Item, int> frequentItems,
                ObjectPool<HashSet<(Item, Item)>> itemsetsPool)
            {
                ItemConverter = itemConverter;
                FrequentItems = frequentItems;
                ItemsetsPool = itemsetsPool;
                ItemsetFrequencies = new ConcurrentDictionary<(Item, Item), int>(parameters.DegreeOfParallelism, 0);
            }

            public void IncrementProcessedTransactionsCount() =>
                Interlocked.Increment(ref _processedTransactionsCount);

            public void Deconstruct(
                out IItemConverter itemConverter,
                out ObjectPool<HashSet<(Item, Item)>> itemsetsPool,
                out IReadOnlyDictionary<Item, int> frequentItems,
                out ConcurrentDictionary<(Item, Item), int> itemsetFrequencies)
            {
                itemConverter = ItemConverter;
                itemsetsPool = ItemsetsPool;
                frequentItems = FrequentItems;
                itemsetFrequencies = ItemsetFrequencies;
            }
        }

        private sealed class SearchForItemsetsStateProvider
        {
            private readonly MiningParameters _parameters;
            private readonly IItemConverter _itemConverter;
            private readonly IReadOnlyDictionary<Item, int> _frequentItems;
            private readonly ObjectPool<HashSet<(Item, Item)>> _itemsetsPool;
            private readonly ConcurrentDictionary<long, SearchForItemsetsState> _states;
            private long _counter;

            public SearchForItemsetsStateProvider(
                MiningParameters parameters,
                IItemConverter itemConverter,
                IReadOnlyDictionary<Item, int> itemFrequencies)
            {
                _parameters = parameters;
                _itemConverter = itemConverter;
                _frequentItems = itemFrequencies;
                _itemsetsPool = new DefaultObjectPool<HashSet<(Item, Item)>>(
                    new ItemsetsPoolPolicy(),
                    parameters.DegreeOfParallelism);
                _states = new ConcurrentDictionary<long, SearchForItemsetsState>();
            }

            public SearchForItemsetsState GetOrCreateState()
            {
                var key = Interlocked.Increment(ref _counter) % _parameters.StatePartitionCount;

                return _states.GetOrAdd(key, ValueFactory);

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                SearchForItemsetsState ValueFactory(long _) =>
                    new SearchForItemsetsState(_parameters, _itemConverter, _frequentItems, _itemsetsPool);
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

        #region Fields and Properties
        private readonly ItemConverterFactory _itemConverterFactory;
        #endregion

        #region Constructors
        internal SearchForItemsetsStep(ItemConverterFactory itemConverterFactory)
        {
            _itemConverterFactory = itemConverterFactory ?? throw new ArgumentNullException(nameof(itemConverterFactory));
        }
        #endregion

        #region Methods
        public SearchForItemsetsResult Run(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if (transactions.IsEmptyCollection())
            {
                return SearchForItemsetsResult.Empty;
            }

            var itemConverter = parameters.ItemConversionRules?.Count > 0
                ? _itemConverterFactory(parameters.ItemConversionRules)
                : null;
            var (frequentItems, transactionsCount) = searchForFrequentItemsResult;
            var stateProvider = new SearchForItemsetsStateProvider(
                parameters,
                itemConverter,
                frequentItems);
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
                    ProcessTransactionBody,
                    _ => { });
            }
            finally
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Dispose();
            }

            stateProvider.AggregateStates(out var itemsetFrequencies);

            return new SearchForItemsetsResult(itemsetFrequencies);

            // ToDo: calculate progress value more accurately
            // ReSharper disable once InconsistentNaming
            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                var progress = 100.0 * stateProvider.GetProcessedTransactionsCount() / transactionsCount;

                miningProgressChangedEventPublisher.Publish(progress);
            }
        }

        public async Task<SearchForItemsetsResult> RunAsync(
            IAsyncEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken)
        {
            var itemConverter = parameters.ItemConversionRules?.Count > 0
                ? _itemConverterFactory(parameters.ItemConversionRules)
                : null;
            var (frequentItems, transactionsCount) = searchForFrequentItemsResult;
            var stateProvider = new SearchForItemsetsStateProvider(parameters, itemConverter, frequentItems);
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
                await ParallelExtensions
                    .ForEachAsync(transactions, stateProvider, parallelOptions, ProcessTransactionBody)
                    .ConfigureAwait(false);
            }
            finally
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Dispose();
            }

            stateProvider.AggregateStates(out var itemsetFrequencies);

            return new SearchForItemsetsResult(itemsetFrequencies);

            // ToDo: calculate progress value more accurately
            // ReSharper disable once InconsistentNaming
            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                var progress = 100.0 * stateProvider.GetProcessedTransactionsCount() / transactionsCount;

                miningProgressChangedEventPublisher.Publish(progress);
            }
        }

        private static SearchForItemsetsState ProcessTransactionBody(
    IReadOnlyList<Item> transaction,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
#pragma warning disable S1172 // Unused method parameters should be removed
    ParallelLoopState _,
#pragma warning restore S1172 // Unused method parameters should be removed
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
    SearchForItemsetsState state) =>
    ProcessTransaction(transaction, state);

        private static void ProcessTransactionBody(
            IReadOnlyList<Item> transaction,
            SearchForItemsetsStateProvider stateProvider,
            CancellationToken cancellationToken) =>
            ProcessTransaction(transaction, stateProvider.GetOrCreateState());

        private static SearchForItemsetsState ProcessTransaction(IReadOnlyList<Item> transaction, SearchForItemsetsState state)
        {
            transaction.ThrowIfNull();

            var (itemConverter, itemsetsPool, itemFrequencies, itemsetFrequencies) = state;
            var itemsets = itemsetsPool.Get();

            try
            {
                for (var i = 0; i < transaction.Count; i++)
                {
                    transaction[i].ThrowIfNull();

                    for (var j = i + 1; j < transaction.Count; j++)
                    {
                        transaction[j].ThrowIfNull();

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
                                                    (itemsets.Contains(itemset) ||
                                                     itemset.Item1.Equals(itemset.Item2));

                            if (shouldSkipItemset)
                            {
                                continue;
                            }

                            itemsets.Add(itemset);
                        }

                        if (itemFrequencies.ContainsKey(itemset.Item1) &&
                            itemFrequencies.ContainsKey(itemset.Item2))
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

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static int UpdateFrequency((Item, Item) _, int frequency) => frequency + 1;
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        #endregion
    }
}