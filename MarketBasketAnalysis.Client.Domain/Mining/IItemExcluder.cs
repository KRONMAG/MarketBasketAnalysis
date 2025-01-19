namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public interface IItemExcluder
    {
        bool ShouldExclude(Item item);
    }
}
