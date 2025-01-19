namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public interface IItemConverter
    {
        ItemsetConversionResult TryConvert(Item item1, Item item2, out Item convertedItem1, out Item convertedItem2);

        bool TryGetGroupItem(Item item, out Item groupItem);
    }
}
