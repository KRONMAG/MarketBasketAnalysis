using System;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.DomainModel;

public sealed class FrequentItem : IEquatable<FrequentItem>
{
    #region Fields and Properties

    public string Name { get; }
    
    public int Count { get; }

    public double Support { get; }

    #endregion Fields and Properties

    #region Constructors

    internal FrequentItem(string name, int count, int transactionCount)
    {
        Contract.RequiresNotNullOrWhiteSpace(name);
        Contract.Requires(count > 0);
        Contract.Requires(count <= transactionCount);

        Name = name;
        Count = count;
        Support = count / (double)transactionCount;
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        Name.GetHashCode(StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        Equals(obj as FrequentItem);

    bool IEquatable<FrequentItem>.Equals(FrequentItem? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override string ToString() =>
        Name;

    #endregion Methods
}