using MathNet.Numerics.Distributions;
using System;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.DomainModel;

public sealed class AssociationRule : IEquatable<AssociationRule>
{
    #region Fields and Properties

    public FrequentItem LeftHandSide { get; }

    public FrequentItem RightHandSide { get; }

    public int Count { get; }

    public double Support { get; }

    public double Confidence { get; }

    public double Lift { get; }

    public double Conviction { get; }

    public double AbsoluteAssociationCoefficient { get; }

    public double AbsoluteContingencyCoefficient { get; }

    public bool AreHandSidesProbablyIndependent { get; }

    #endregion Fields and Properties

    #region Constructors

    internal AssociationRule(string leftHandSideName, string rightHandSideName, int leftHandSideCount,
        int rightHandSideCount, int itemsetCount, int transactionCount)
    {
        Contract.RequiresNotNullOrWhiteSpace(leftHandSideName);
        Contract.RequiresNotNullOrWhiteSpace(rightHandSideName);
        Contract.Requires(!leftHandSideName.Equals(rightHandSideName, StringComparison.Ordinal));
        Contract.Requires(leftHandSideCount > 0);
        Contract.Requires(rightHandSideCount > 0);
        Contract.Requires(itemsetCount > 0);
        Contract.Requires(transactionCount > 0);
        Contract.Requires(leftHandSideCount <= transactionCount);
        Contract.Requires(rightHandSideCount <= transactionCount);
        Contract.Requires(itemsetCount <= Math.Min(leftHandSideCount, rightHandSideCount));

        LeftHandSide = new FrequentItem(leftHandSideName, leftHandSideCount, transactionCount);
        RightHandSide = new FrequentItem(rightHandSideName, rightHandSideCount, transactionCount);

        Count = itemsetCount;
        Support = itemsetCount / (double)transactionCount;
        Confidence = Support / LeftHandSide.Support;
        Lift = Confidence / RightHandSide.Support;
        Conviction = (1 - RightHandSide.Support) / (1 - Confidence);

        var a = itemsetCount;
        var b = leftHandSideCount - itemsetCount;
        var c = rightHandSideCount - itemsetCount;
        var d = transactionCount - a - b - c;

        AbsoluteAssociationCoefficient = Math.Abs((a * d - b * c) / (double)(a * d + b * c));
        AbsoluteContingencyCoefficient = Math.Abs((a * d - b * c) / Math.Sqrt((a + b) * (a + c) * (b + d) * (c + d)));

        var chiSquaredValue = itemsetCount * Math.Pow(a * d - b * c, 2) / ((a + b) * (a + c) * (b + d) * (c + d));

        AreHandSidesProbablyIndependent = chiSquaredValue < ChiSquared.InvCDF(1, 0.99);
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        unchecked(LeftHandSide.GetHashCode() * 397 ^ RightHandSide.GetHashCode());

    public override bool Equals(object? obj) =>
        Equals(obj as AssociationRule);

    bool IEquatable<AssociationRule>.Equals(AssociationRule? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return LeftHandSide.Equals(other.LeftHandSide) && RightHandSide.Equals(other.RightHandSide);
    }

    public override string ToString() =>
        $"{LeftHandSide.Name} -> {RightHandSide.Name}";

    #endregion Methods
}