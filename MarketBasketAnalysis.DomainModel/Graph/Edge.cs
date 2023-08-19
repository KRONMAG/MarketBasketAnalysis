using MarketBasketAnalysis.DomainModel.AssociationRules;
using QuickGraph;
using System;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.DomainModel.Graph;

public sealed class Edge : IEdge<Vertex>, IEquatable<Edge>
{
    #region Fields and Properties

    public Vertex Source { get; }

    public Vertex Target { get; }

    public AssociationRule AssociationRule { get; }

    #endregion Fields and Properties

    #region Constructors

    public Edge(AssociationRule rule)
    {
        Contract.RequiresNotNull(rule);

        Source = new Vertex(rule.LeftHandSide);
        Target = new Vertex(rule.RightHandSide);

        AssociationRule = rule;
    }

    #endregion Constructors

    #region Methods

    public override int GetHashCode() =>
        AssociationRule.GetHashCode();

    public override bool Equals(object? obj) =>
        Equals(obj as Edge);

    bool IEquatable<Edge>.Equals(Edge? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return AssociationRule.Equals(other.AssociationRule);
    }

    public override string ToString() =>
        AssociationRule.ToString();

    #endregion Methods
}