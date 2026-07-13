// Ignore Spelling: Excluder

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    /// <inheritdoc cref="IMiner" />
    internal sealed class Miner : IMiner, IMiningProgressChangedEventPublisher
    {
        #region Fields and Properties
        private readonly ISearchForFrequentItemsStep _searchForFrequentItemsStep;
        private readonly ISearchForItemsetsStep _searchForItemsetsStep;
        private readonly IGenerateAssociationRulesStep _generateAssociationRulesStep;

        /// <inheritdoc />
        public event EventHandler<MiningProgressChangedEventArgs> MiningProgressUpdated;

        /// <inheritdoc />
        public event EventHandler<MiningStageChangedEventArgs> MiningStageChanged;
        #endregion

        #region Constructors
        internal Miner(
            ISearchForFrequentItemsStep searchForFrequentItemsStep,
            ISearchForItemsetsStep searchForItemsetsStep,
            IGenerateAssociationRulesStep generateAssociationRulesStep)
        {
            _searchForFrequentItemsStep = searchForFrequentItemsStep ?? throw new ArgumentNullException(nameof(searchForFrequentItemsStep));
            _searchForItemsetsStep = searchForItemsetsStep ?? throw new ArgumentNullException(nameof(searchForItemsetsStep));
            _generateAssociationRulesStep = generateAssociationRulesStep ?? throw new ArgumentNullException(nameof(generateAssociationRulesStep));
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

            OnMiningStageChanged(MiningStage.FrequentItemSearch);

            var searchForFrequentItemsResult = _searchForFrequentItemsStep.Run(
                transactions, parameters, cancellationToken);

            OnMiningStageChanged(MiningStage.ItemsetSearch);

            var searchForItemsetsResult = _searchForItemsetsStep.Run(
                transactions, parameters, searchForFrequentItemsResult, this, cancellationToken);

            OnMiningStageChanged(MiningStage.AssociationRuleGeneration);

            var generateAssociationRulesResult = _generateAssociationRulesStep.Run(
                searchForFrequentItemsResult, searchForItemsetsResult, parameters, cancellationToken);

            return generateAssociationRulesResult.AssociationRules;
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Possibility of multiple enumeration is specified in docs for IMiner.")]
        public async Task<IReadOnlyCollection<AssociationRule>> MineAsync(
            IAsyncEnumerable<IReadOnlyList<Item>> transactions,
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

            OnMiningStageChanged(MiningStage.FrequentItemSearch);

            var searchForFrequentItemsResult = await _searchForFrequentItemsStep
                .RunAsync(transactions, parameters, cancellationToken)
                .ConfigureAwait(false);

            OnMiningStageChanged(MiningStage.ItemsetSearch);

            var searchForItemsetsResult = await _searchForItemsetsStep
                .RunAsync(transactions, parameters, searchForFrequentItemsResult, this, cancellationToken)
                .ConfigureAwait(false);

            OnMiningStageChanged(MiningStage.AssociationRuleGeneration);

            var generateAssociationRulesResult = _generateAssociationRulesStep.Run(
                searchForFrequentItemsResult, searchForItemsetsResult, parameters, cancellationToken);

            return generateAssociationRulesResult.AssociationRules;
        }

        void IMiningProgressChangedEventPublisher.Publish(double progress) => OnMiningProgressChanged(progress);

        private void OnMiningStageChanged(MiningStage stage) =>
            MiningStageChanged?.Invoke(this, new MiningStageChangedEventArgs(stage));

        private void OnMiningProgressChanged(double progress) =>
            MiningProgressUpdated?.Invoke(this, new MiningProgressChangedEventArgs(progress));
        #endregion
    }
}