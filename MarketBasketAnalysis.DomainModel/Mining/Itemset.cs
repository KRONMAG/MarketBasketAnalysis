using System;
using System.Diagnostics.ContractsLight;
using static System.StringComparison;

namespace MarketBasketAnalysis.DomainModel.Mining;

public sealed class Itemset : IEquatable<Itemset>
{
    #region Fields and Properties

    public string FirstItem { get; }

    public string SecondItem { get; }

    #endregion Fields and Properties

    #region Constructors

    public Itemset(string item1, string item2)
    {
        Contract.RequiresNotNullOrWhiteSpace(item1);
        Contract.RequiresNotNullOrWhiteSpace(item2);
        Contract.Requires(!item1.Equals(item2, Ordinal));

        FirstItem = item1;
        SecondItem = item2;
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        unchecked(FirstItem.GetHashCode(Ordinal) * 397 ^ SecondItem.GetHashCode(Ordinal));

    public override bool Equals(object? obj) =>
        Equals(obj as Itemset);

    public bool Equals(Itemset? other)
    {
        if (ReferenceEquals(other, null))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return FirstItem.Equals(other.FirstItem, Ordinal) &&
            SecondItem.Equals(other.SecondItem, Ordinal);
    }

    #endregion Methods
}