// Ignore Spelling: Excluder

using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface IItemExcluder
    {
        bool ShouldExclude(Item item);
    }
}