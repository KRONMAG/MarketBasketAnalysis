using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class GenerateAssociationRulesStep : IGenerateAssociationRulesStep
    {
        #region Nested Types
        private sealed class LocalState
        {
            public MiningParameters Parameters { get; }

            public IReadOnlyDictionary<Item, int> FrequentItems { get; }

            public int TransactionsCount { get; }

            public int FrequencyThreshold { get; }

            public ConcurrentBag<AssociationRule> AssociationRules { get; }

            public LocalState(
                MiningParameters parameters,
                IReadOnlyDictionary<Item, int> frequentItems,
                int transactionsCount)
            {
                Parameters = parameters;
                FrequentItems = frequentItems;
                TransactionsCount = transactionsCount;
                FrequencyThreshold = (int)Math.Ceiling(transactionsCount * parameters.MinSupport);
                AssociationRules = new ConcurrentBag<AssociationRule>();
            }

            public void Deconstruct(
                out MiningParameters parameters,
                out IReadOnlyDictionary<Item, int> frequentItems,
                out int transactionsCount,
                out int frequencyThreshold,
                out ConcurrentBag<AssociationRule> associationRules)
            {
                parameters = Parameters;
                frequentItems = FrequentItems;
                transactionsCount = TransactionsCount;
                frequencyThreshold = FrequencyThreshold;
                associationRules = AssociationRules;
            }
        }
        #endregion

        #region Methods
        public GenerateAssociationRulesResult Run(
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            SearchForFrequentPairsResult searchForFrequentPairsResult,
            MiningParameters parameters,
            CancellationToken cancellationToken)
        {
            var (frequentItems, transactionsCount) = searchForFrequentItemsResult;
            var frequentPairs = searchForFrequentPairsResult.FrequentPairs;

            var localState = new LocalState(parameters, frequentItems, transactionsCount);
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.MaxDegreeOfParallelism,
            };

            Parallel.ForEach(frequentPairs, parallelOptions, () => localState, GenerateAssociationRulePair, _ => { });

            return new GenerateAssociationRulesResult(localState.AssociationRules);
        }

        private static LocalState GenerateAssociationRulePair(
            KeyValuePair<(Item, Item), int> keyValuePair,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
            ParallelLoopState _,
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            LocalState state)
        {
            var (parameters, frequentItems, transactionsCount, frequencyThreshold, associationRules) = state;
            var itemsetFrequency = keyValuePair.Value;

            if (itemsetFrequency < frequencyThreshold)
            {
                return state;
            }

            var (item1, item2) = keyValuePair.Key;
            var item1Frequency = frequentItems[item1];
            var item2Frequency = frequentItems[item2];

            if (itemsetFrequency / (double)item1Frequency >= parameters.MinConfidence)
            {
                associationRules.Add(new AssociationRule(
                    item1,
                    item2,
                    item1Frequency,
                    item2Frequency,
                    itemsetFrequency,
                    transactionsCount));
            }

            if (itemsetFrequency / (double)item2Frequency >= parameters.MinConfidence)
            {
                associationRules.Add(
                    new AssociationRule(
                        item2,
                        item1,
                        item2Frequency,
                        item1Frequency,
                        itemsetFrequency,
                        transactionsCount));
            }

            return state;
        }
        #endregion
    }
}