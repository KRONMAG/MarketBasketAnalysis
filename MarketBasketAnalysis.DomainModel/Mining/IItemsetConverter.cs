using System.Diagnostics.CodeAnalysis;

namespace MarketBasketAnalysis.DomainModel.Mining;

public interface IItemsetConverter
{
    ItemsetConversionResult TryConvert(Itemset itemset, out Itemset? convertedItemset);

    bool TryConvert(string item, [NotNullWhen(true)] out string? convertedItem);
}
