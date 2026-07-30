using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface IItemConverter
    {
        bool TryConvert(Item item, out Item group);
    }
}
