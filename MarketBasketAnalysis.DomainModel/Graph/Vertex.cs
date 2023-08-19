using System;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.DomainModel.Graph;

public sealed class Vertex : IEquatable<Vertex>
{
    #region Fields and Properties

    public FrequentItem FrequentItem { get; }

    #endregion Fields and Properties

    #region Constructors

    public Vertex(FrequentItem frequentItem)
    {
        Contract.RequiresNotNull(frequentItem);

        FrequentItem = frequentItem;
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        FrequentItem.GetHashCode();

    public override bool Equals(object? obj) =>
        Equals(obj as Vertex);

    bool IEquatable<Vertex>.Equals(Vertex? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return FrequentItem.Equals(other.FrequentItem);
    }

    public override string ToString() =>
        FrequentItem.ToString();

    #endregion Methods
}