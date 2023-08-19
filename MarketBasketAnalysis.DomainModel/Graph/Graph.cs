using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using QuickGraph;
using MarketBasketAnalysis.DomainModel.AssociationRules;
#pragma warning disable CA1724

namespace MarketBasketAnalysis.DomainModel.Graph;

public sealed class Graph : BidirectionalGraph<Vertex, Edge>
{
    public Graph(IReadOnlyList<AssociationRule> rules)
    {
        Contract.RequiresNotNull(rules);
        Contract.ForAll(rules, rule => rule != null);

        AddVerticesAndEdgeRange(rules.Select(rule => new Edge(rule)));
    }

    public Graph()
    {
        
    }
}