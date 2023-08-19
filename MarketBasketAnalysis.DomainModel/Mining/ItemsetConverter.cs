using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.ContractsLight;
using System.Linq;
using MarketBasketAnalysis.DomainModel.Mining;
using static MarketBasketAnalysis.DomainModel.Mining.ItemsetConversionResult;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining;

public sealed class ItemsetConverter : IItemsetConverter
{
    #region Fields and Properties

    private readonly IReadOnlyDictionary<string, string> _replacementRules;

    #endregion Fields and Properties

    #region Constructors

    public ItemsetConverter(IReadOnlySet<ItemConversionRule> conversionRules)
    {
        Contract.RequiresNotNull(conversionRules);
        Contract.RequiresForAll(conversionRules, item => item != null);
        Contract.Requires(conversionRules.Select(item => item.Item).Count() == conversionRules.Count);
        Contract.Requires(!conversionRules.Select(item => item.Item)
            .Intersect(conversionRules.Select(item => item.Group)).Any());

        _replacementRules = conversionRules.ToDictionary(rule => rule.Item, rule => rule.Group);
    }

    #endregion Constructors

    #region Methods

    public ItemsetConversionResult TryConvert(Itemset itemset, out Itemset? convertedItemset)
    {
        Contract.RequiresNotNull(itemset);

        convertedItemset = null;

        _replacementRules.TryGetValue(itemset.FirstItem, out var leftHandSideGroup);
        _replacementRules.TryGetValue(itemset.SecondItem, out var rightHandSideGroup);

        if (leftHandSideGroup == null && rightHandSideGroup == null)
            return NoConversionRequired;

        if (leftHandSideGroup?.Equals(rightHandSideGroup, StringComparison.Ordinal) == true)
            return ConvertedItemsetHasSameItems;

        convertedItemset = new Itemset(leftHandSideGroup ?? itemset.FirstItem,
            rightHandSideGroup ?? itemset.SecondItem);

        return ItemsetConverted;
    }

    public bool TryConvert(string item, [NotNullWhen(true)] out string? convertedItem)
    {
        Contract.RequiresNotNullOrWhiteSpace(item);

        return _replacementRules.TryGetValue(item, out convertedItem);
    }

    #endregion Methods
}