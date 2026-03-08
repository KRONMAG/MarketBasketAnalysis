// Ignore Spelling: Excluder

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Timers;
using Microsoft.Extensions.ObjectPool;
using Timer = System.Timers.Timer;

namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    internal sealed partial class Miner : IMiner
    {
        #region Fields and Properties
        private readonly Func<IReadOnlyCollection<ItemConversionRule>, IItemConverter> _itemConverterFactory;
        private readonly Func<IReadOnlyCollection<ItemExclusionRule>, IItemExcluder> _itemExcluderFactory;

        private IItemConverter _itemConverter;

        /// <inheritdoc />
        public event EventHandler<MiningProgressChangedEventArgs> MiningProgressChanged;

        /// <inheritdoc />
        public event EventHandler<MiningStageChangedEventArgs> MiningStageChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Miner"/> class.
        /// </summary>
        /// <param name="itemConverterFactory">
        /// A factory function that creates a <see cref="IItemConverter"/> based on a collection of <see cref="ItemConversionRule"/>.
        /// This is used to define how items are grouped or replaced during mining.
        /// </param>
        /// <param name="itemExcluderFactory">
        /// A factory function that creates a <see cref="IItemExcluder"/> based on a collection of <see cref="ItemExclusionRule"/>.
        /// This is used to define which items or groups should be excluded from mining.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="itemConverterFactory"/> or <paramref name="itemExcluderFactory"/> is <c>null</c>.
        /// </exception>
        public Miner(
            Func<IReadOnlyCollection<ItemConversionRule>, IItemConverter> itemConverterFactory,
            Func<IReadOnlyCollection<ItemExclusionRule>, IItemExcluder> itemExcluderFactory)
        {
            _itemConverterFactory = itemConverterFactory ?? throw new ArgumentNullException(nameof(itemConverterFactory));
            _itemExcluderFactory = itemExcluderFactory ?? throw new ArgumentNullException(nameof(itemExcluderFactory));
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Possibility of multiple enumeration is specified in docs for IMiner.")]
        public IReadOnlyCollection<AssociationRule> Mine(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            CancellationToken token = default)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                _itemConverter = parameters.ItemConversionRules != null
                    ? _itemConverterFactory(parameters.ItemConversionRules)
                    : null;
                var itemExcluder = parameters.ItemExclusionRules != null
                    ? _itemExcluderFactory(parameters.ItemExclusionRules)
                    : null;

                OnMiningStageChanged(MiningStage.FrequentItemSearch);

                var frequentItems = SearchForFrequentItems(
                    transactions,
                    parameters,
                    itemExcluder,
                    _itemConverter,
                    token,
                    out var transactionCount);

                OnMiningStageChanged(MiningStage.ItemsetSearch);

                var itemsets = SearchForItemsets(transactions, parameters, frequentItems, transactionCount, token);

                OnMiningStageChanged(MiningStage.AssociationRuleGeneration);

                return GenerateAssociationRules(itemsets, frequentItems, transactionCount, parameters, token);
            }
            finally
            {
                _itemConverter = null;
            }
        }

        private static void ThrowIfTransactionIsNull(IReadOnlyList<Item> transaction)
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction cannot be null.");
            }
        }

        private static void UpdateFrequency<TKey>(Dictionary<TKey, int> frequencies, TKey key)
        {
            if (frequencies.TryGetValue(key, out var value))
            {
                frequencies[key] = value + 1;
            }
            else
            {
                frequencies[key] = 1;
            }
        }

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static int UpdateFrequency<TKey>(TKey _, int frequency) => frequency + 1;
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter

        private ConcurrentDictionary<(Item, Item), int> SearchForItemsets(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            Dictionary<Item, int> frequentItems,
            int transactionCount,
            CancellationToken token)
        {
            var previousProcessedTransactionsCount = 0;
            var processedTransactionCount = 0;

            // ToDo: calculate progress value more accurately
            var timer = new Timer(100);

            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            var itemsetFrequencies = new ConcurrentDictionary<(Item, Item), int>(parameters.DegreeOfParallelism, 0);
            var itemsetsPool = new DefaultObjectPool<HashSet<(Item, Item)>>(
                new DefaultPooledObjectPolicy<HashSet<(Item, Item)>>(),
                parameters.DegreeOfParallelism);

            try
            {
                transactions
                    .AsParallel()
                    .WithCancellation(token)
                    .WithDegreeOfParallelism(parameters.DegreeOfParallelism)
                    .ForAll(transaction =>
                    {
                        ThrowIfTransactionIsNull(transaction);

                        var itemsets = itemsetsPool.Get();

                        for (var i = 0; i < transaction.Count; i++)
                        {
                            for (var j = i + 1; j < transaction.Count; j++)
                            {
                                var itemset = transaction[i].Id < transaction[j].Id
                                    ? (transaction[i], transaction[j])
                                    : (transaction[j], transaction[i]);

                                if (itemset.Item1.Equals(itemset.Item2) || !itemsets.Add(itemset))
                                {
                                    continue;
                                }

                                if (_itemConverter != null)
                                {
                                    var isItem1Converted = _itemConverter.TryConvert(itemset.Item1, out var item1Group);
                                    var isItem2Converted = _itemConverter.TryConvert(itemset.Item2, out var item2Group);

                                    if (isItem1Converted)
                                    {
                                        itemset.Item1 = item1Group;
                                    }

                                    if (isItem2Converted)
                                    {
                                        itemset.Item2 = item2Group;
                                    }

                                    var shouldSkipItemset = (isItem1Converted || isItem2Converted) &&
                                        (!itemsets.Add(itemset) || itemset.Item1.Equals(itemset.Item2));

                                    if (shouldSkipItemset)
                                    {
                                        continue;
                                    }
                                }

                                if (frequentItems.ContainsKey(itemset.Item1) && frequentItems.ContainsKey(itemset.Item2))
                                {
                                    itemsetFrequencies.AddOrUpdate(itemset, 1, UpdateFrequency);
                                }
                            }
                        }

                        itemsets.Clear();
                        itemsetsPool.Return(itemsets);

                        processedTransactionCount++;
                    });
            }
            finally
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Dispose();
            }

            return itemsetFrequencies;

            // ReSharper disable once InconsistentNaming
            void Timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (processedTransactionCount == previousProcessedTransactionsCount)
                {
                    return;
                }

                var progress = processedTransactionCount / (double)transactionCount * 100;

                previousProcessedTransactionsCount = processedTransactionCount;

                OnMiningProgressChanged(progress);
            }
        }

        private void OnMiningStageChanged(MiningStage stage) =>
            MiningStageChanged?.Invoke(this, new MiningStageChangedEventArgs(stage));

        private void OnMiningProgressChanged(double progress) =>
            MiningProgressChanged?.Invoke(this, new MiningProgressChangedEventArgs(progress));

        private ConcurrentBag<AssociationRule> GenerateAssociationRules(
            ConcurrentDictionary<(Item, Item), int> frequentItemsets,
            Dictionary<Item, int> frequentItems,
            int transactionCount,
            MiningParameters parameters,
            CancellationToken token)
        {
            var frequencyThreshold = (int)Math.Ceiling(transactionCount * parameters.MinSupport);
            var associationRules = new ConcurrentBag<AssociationRule>();

            frequentItemsets
                .AsParallel()
                .WithDegreeOfParallelism(parameters.DegreeOfParallelism)
                .WithCancellation(token)
                .ForAll(GenerateAssociationRulePair);

            void GenerateAssociationRulePair(KeyValuePair<(Item, Item), int> keyValuePair)
            {
                var itemsetFrequency = keyValuePair.Value;

                if (itemsetFrequency < frequencyThreshold)
                {
                    return;
                }

                var (item1, item2) = keyValuePair.Key;
                var itemFrequency1 = frequentItems[item1];
                var itemFrequency2 = frequentItems[item2];

                if (itemsetFrequency / (double)itemFrequency1 >= parameters.MinConfidence)
                {
                    associationRules.Add(new AssociationRule(
                        item1,
                        item2,
                        itemFrequency1,
                        itemFrequency2,
                        itemsetFrequency,
                        transactionCount));
                }

                if (itemsetFrequency / (double)itemFrequency2 >= parameters.MinConfidence)
                {
                    associationRules.Add(
                        new AssociationRule(
                            item2,
                            item1,
                            itemFrequency2,
                            itemFrequency1,
                            itemsetFrequency,
                            transactionCount));
                }
            }

            return associationRules;
        }
        #endregion
    }
}