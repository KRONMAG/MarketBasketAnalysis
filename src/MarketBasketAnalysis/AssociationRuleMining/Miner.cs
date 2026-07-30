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
    internal sealed class Miner : IMiner, IMiningProgressChangedEventPublisher
    {
        #region Fields and Properties
        private readonly ISearchForFrequentItemsStep _searchForFrequentItemsStep;
        private readonly ISearchForFrequentPairsStep _searchForItemsetsStep;
        private readonly IGenerateAssociationRulesStep _generateAssociationRulesStep;

        public event EventHandler<MiningProgressChangedEventArgs> MiningProgressChanged;

        public event EventHandler<MiningStepStartedEventArgs> MiningStepStarted;
        #endregion

        #region Constructors
        internal Miner(
            ISearchForFrequentItemsStep searchForFrequentItemsStep,
            ISearchForFrequentPairsStep searchForItemsetsStep,
            IGenerateAssociationRulesStep generateAssociationRulesStep)
        {
            _searchForFrequentItemsStep = searchForFrequentItemsStep ?? throw new ArgumentNullException(nameof(searchForFrequentItemsStep));
            _searchForItemsetsStep = searchForItemsetsStep ?? throw new ArgumentNullException(nameof(searchForItemsetsStep));
            _generateAssociationRulesStep = generateAssociationRulesStep ?? throw new ArgumentNullException(nameof(generateAssociationRulesStep));
        }
        #endregion

        #region Methods
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

            OnMiningStepStarted(MiningStep.SearchForFrequentItems);

            var searchForFrequentItemsResult = _searchForFrequentItemsStep.Run(
                transactions, parameters, cancellationToken);

            OnMiningStepStarted(MiningStep.SearchForFrequentPairs);

            var searchForItemsetsResult = _searchForItemsetsStep.Run(
                transactions, parameters, searchForFrequentItemsResult, this, cancellationToken);

            OnMiningStepStarted(MiningStep.GenerateAssociationRules);

            var generateAssociationRulesResult = _generateAssociationRulesStep.Run(
                searchForFrequentItemsResult, searchForItemsetsResult, parameters, cancellationToken);

            return generateAssociationRulesResult.AssociationRules;
        }

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

            OnMiningStepStarted(MiningStep.SearchForFrequentItems);

            var searchForFrequentItemsResult = await _searchForFrequentItemsStep
                .RunAsync(transactions, parameters, cancellationToken)
                .ConfigureAwait(false);

            OnMiningStepStarted(MiningStep.SearchForFrequentPairs);

            var searchForItemsetsResult = await _searchForItemsetsStep
                .RunAsync(transactions, parameters, searchForFrequentItemsResult, this, cancellationToken)
                .ConfigureAwait(false);

            OnMiningStepStarted(MiningStep.GenerateAssociationRules);

            var generateAssociationRulesResult = _generateAssociationRulesStep.Run(
                searchForFrequentItemsResult, searchForItemsetsResult, parameters, cancellationToken);

            return generateAssociationRulesResult.AssociationRules;
        }

        void IMiningProgressChangedEventPublisher.Publish(double progress) => OnMiningProgressChanged(progress);

        private void OnMiningStepStarted(MiningStep step) =>
            MiningStepStarted?.Invoke(this, new MiningStepStartedEventArgs(step));

        private void OnMiningProgressChanged(double progress) =>
            MiningProgressChanged?.Invoke(this, new MiningProgressChangedEventArgs(progress));
        #endregion
    }
}