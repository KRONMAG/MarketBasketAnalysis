using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Mining
{
    internal sealed partial class Miner
    {
        #region Nested types
        private sealed class GenerateAssociationRulesState
        {
            public MiningParameters Parameters { get; }

            public Dictionary<Item, int> FrequentItems { get; }

            public int TransactionsCount { get; }

            public int FrequencyThreshold { get; }

            public ConcurrentBag<AssociationRule> AssociationRules { get; }

            public GenerateAssociationRulesState(
                MiningParameters parameters,
                Dictionary<Item, int> frequentItems,
                int transactionsCount)
            {
                Parameters = parameters;
                FrequentItems = frequentItems;
                TransactionsCount = transactionsCount;
                FrequencyThreshold = (int)Math.Ceiling(transactionsCount * parameters.MinSupport);
                AssociationRules = new ConcurrentBag<AssociationRule>();
            }

            public void Deconstruct(
                out Dictionary<Item, int> frequentItems,
                out int transactionsCount,
                out MiningParameters parameters,
                out int frequencyThreshold,
                out ConcurrentBag<AssociationRule> associationRules)
            {
                frequentItems = FrequentItems;
                transactionsCount = TransactionsCount;
                parameters = Parameters;
                frequencyThreshold = FrequencyThreshold;
                associationRules = AssociationRules;
            }
        }
        #endregion

        #region Methods
        private static ConcurrentBag<AssociationRule> GenerateAssociationRules(
            MiningParameters parameters,
            Dictionary<Item, int> frequentItems,
            IReadOnlyDictionary<(Item, Item), int> frequentItemsets,
            int transactionsCount,
            CancellationToken cancellationToken)
        {
            var state = new GenerateAssociationRulesState(parameters, frequentItems, transactionsCount);
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parameters.DegreeOfParallelism,
            };

            Parallel.ForEach(frequentItemsets, parallelOptions, () => state, GenerateAssociationRulePair, _ => { });

            return state.AssociationRules;
        }

        private static GenerateAssociationRulesState GenerateAssociationRulePair(
            KeyValuePair<(Item, Item), int> keyValuePair,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
            ParallelLoopState _,
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
            GenerateAssociationRulesState state)
        {
            var (frequentItems, transactionsCount, parameters, frequencyThreshold, associationRules) = state;
            var itemsetFrequency = keyValuePair.Value;

            if (itemsetFrequency < frequencyThreshold)
            {
                return state;
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
                    transactionsCount));
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
                        transactionsCount));
            }

            return state;
        }
        #endregion
    }
}