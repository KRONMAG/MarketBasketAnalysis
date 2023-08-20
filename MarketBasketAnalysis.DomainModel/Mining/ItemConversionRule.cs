using System;
using System.Diagnostics.ContractsLight;
using static System.StringComparison;

namespace MarketBasketAnalysis.DomainModel.Mining;

public sealed class ItemConversionRule : IEquatable<ItemConversionRule>
{
    #region Fields and Properties

    public string Item { get; }

    public string Group { get; }

    #endregion Fields and Properties

    #region Constructors

    public ItemConversionRule(string item, string group)
    {
        Contract.RequiresNotNullOrWhiteSpace(item);
        Contract.RequiresNotNullOrWhiteSpace(group);
        Contract.Requires(!item.Equals(group, Ordinal));

        Item = item;
        Group = group;
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        unchecked((Item.GetHashCode(Ordinal) * 397) ^ Group.GetHashCode(Ordinal));

    public override bool Equals(object? obj) =>
        Equals(obj as ItemConversionRule);

    bool IEquatable<ItemConversionRule>.Equals(ItemConversionRule? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Item.Equals(other.Item, Ordinal) && Group.Equals(other.Group, Ordinal);
    }

    #endregion Methods
}