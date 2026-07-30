using System.Collections.Generic;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal delegate IItemConverter ItemConverterFactory(IReadOnlyCollection<ItemConversionRule> itemConversionRules);
}