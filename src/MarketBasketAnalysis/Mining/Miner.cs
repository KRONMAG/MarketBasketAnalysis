// Ignore Spelling: Excluder

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    internal sealed partial class Miner : IMiner
    {
        #region Fields and Properties
        private readonly Func<IReadOnlyCollection<ItemConversionRule>, IItemConverter> _itemConverterFactory;
        private readonly Func<IReadOnlyCollection<ItemExclusionRule>, IItemExcluder> _itemExcluderFactory;

        /// <inheritdoc />
        public event EventHandler<MiningProgressChangedEventArgs> MiningProgressUpdated;

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
            CancellationToken cancellationToken = default)
        {
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var itemExcluder = parameters.ItemExclusionRules != null
                ? _itemExcluderFactory(parameters.ItemExclusionRules)
                : null;
            var itemConverter = parameters.ItemConversionRules != null
                ? _itemConverterFactory(parameters.ItemConversionRules)
                : null;

            OnMiningStageChanged(MiningStage.FrequentItemSearch);

            var frequentItems = SearchForFrequentItems(
                transactions,
                parameters,
                itemExcluder,
                itemConverter,
                cancellationToken,
                out var transactionCount);

            OnMiningStageChanged(MiningStage.ItemsetSearch);

            var itemsets = SearchForItemsets(
                transactions,
                parameters,
                itemConverter,
                frequentItems,
                transactionCount,
                cancellationToken);

            OnMiningStageChanged(MiningStage.AssociationRuleGeneration);

            return GenerateAssociationRules(
                parameters,
                frequentItems,
                itemsets,
                transactionCount,
                cancellationToken);
        }

        private static void ThrowIfTransactionIsNull(IReadOnlyList<Item> transaction)
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction cannot be null.");
            }
        }

        private static void ThrowIfItemIsNull(Item item)
        {
            if (item == null)
            {
                throw new InvalidOperationException("Item cannot be null.");
            }
        }

        private static bool IsTransactionsEmptyCollection(IEnumerable<IReadOnlyCollection<Item>> transactions) =>
            (transactions is ICollection<IReadOnlyList<Item>> collection &&
             collection.Count == 0) ||
            (transactions is IReadOnlyCollection<IReadOnlyList<Item>> readOnlyCollection &&
             readOnlyCollection.Count == 0);

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
        private static int UpdateFrequency<TKey>(TKey _, int frequency) => frequency + 1;
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter

        private void OnMiningStageChanged(MiningStage stage) =>
            MiningStageChanged?.Invoke(this, new MiningStageChangedEventArgs(stage));

        private void OnMiningProgressChanged(double progress) =>
            MiningProgressUpdated?.Invoke(this, new MiningProgressChangedEventArgs(progress));
        #endregion
    }
}