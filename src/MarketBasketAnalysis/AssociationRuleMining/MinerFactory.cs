using System;
using System.Collections.Generic;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    /// <inheritdoc />
    internal sealed class MinerFactory : IMinerFactory
    {
        /// <inheritdoc />
        public IMiner Create()
        {
            return new Miner(
                new SearchForFrequentItemsStep(CreateItemExcluder, CreateItemConverter),
                new SearchForItemsetsStep(CreateItemConverter),
                new GenerateAssociationRulesStep());
        }

        private static IItemExcluder CreateItemExcluder(IReadOnlyCollection<ItemExclusionRule> itemExclusionRules)
        {
            if (itemExclusionRules == null)
            {
                throw new ArgumentNullException(nameof(itemExclusionRules));
            }

            return new ItemExcluder(itemExclusionRules);
        }

        private static IItemConverter CreateItemConverter(IReadOnlyCollection<ItemConversionRule> itemConversionRules)
        {
            if (itemConversionRules == null)
            {
                throw new ArgumentNullException(nameof(itemConversionRules));
            }

            return new ItemConverter(itemConversionRules);
        }
    }
}