using System;
using System.Diagnostics.ContractsLight;
using static System.StringComparison;

namespace MarketBasketAnalysis.DomainModel.AssociationRules.Mining;

public class ItemExclusionRule : IEquatable<ItemExclusionRule>
{
    #region Fields and Properties

    public string ItemPattern { get; }

    public bool ExactMatch { get; }

    public bool IgnoreCase { get; }

    #endregion Fields and Properties

    #region Constructors

    public ItemExclusionRule(string itemPattern, bool exactMatch, bool ignoreCase)
    {
        Contract.RequiresNotNull(itemPattern);

        ItemPattern = itemPattern;
        ExactMatch = exactMatch;
        IgnoreCase = ignoreCase;
    }

    #endregion Constructors

    #region Methods

    public bool ShouldExclude(string item)
    {
        Contract.RequiresNotNullOrWhiteSpace(item);

        return (ExactMatch, IgnoreCase) switch
        {
            (false, false) => item.Contains(ItemPattern, Ordinal),
            (true, false) => string.Equals(item, ItemPattern, Ordinal),
            (false, true) => item.Contains(ItemPattern, OrdinalIgnoreCase),
            (true, true) => string.Equals(item, ItemPattern, OrdinalIgnoreCase)
        };
    }

    public override int GetHashCode() =>
        ItemPattern.GetHashCode(Ordinal);

    public override bool Equals(object? obj) =>
        Equals(obj as ItemExclusionRule);

    bool IEquatable<ItemExclusionRule>.Equals(ItemExclusionRule? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return ItemPattern.Equals(other.ItemPattern, Ordinal) &&
            ExactMatch == other.ExactMatch &&
            IgnoreCase == other.IgnoreCase;
    }

    #endregion Methods
}