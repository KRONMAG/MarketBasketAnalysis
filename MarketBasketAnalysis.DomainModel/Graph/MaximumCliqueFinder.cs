using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Linq;

namespace MarketBasketAnalysis.DomainModel.Graph;

public sealed class MaximumCliqueFinder : IMaximumCliqueFinder
{
    #region Fields and Properties

    private BronKerboschAlgorithm<Vertex, UndirectedEdge<Vertex>>? _algorithm;

    public event EventHandler? Aborted;

    #endregion Fields and Properties

    #region Methods

    public IReadOnlyCollection<Graph> Find(Graph graph, int minCliqueSize, int maxCliqueSize, bool considerDirection)
    {
        Contract.RequiresNotNull(graph);
        Contract.Requires(minCliqueSize > 1);
        Contract.Requires(maxCliqueSize > 1);
        Contract.Requires(minCliqueSize <= maxCliqueSize);

        var directedEdges = graph.Edges.ToDictionary(edge => (edge.Source, edge.Target));
        var undirectedEdges = new HashSet<(Vertex Source, Vertex Target)>();

        foreach (var (directedEdge, _) in directedEdges)
        {
            var shouldToAddEdge = !undirectedEdges.Contains(directedEdge) &&
                !undirectedEdges.Contains((directedEdge.Target, directedEdge.Source)) &&
                (!considerDirection || directedEdges.ContainsKey((directedEdge.Target, directedEdge.Source)));

            if (shouldToAddEdge)
                undirectedEdges.Add(directedEdge);
        }

        var undirectedGraph = new UndirectedGraph<Vertex, UndirectedEdge<Vertex>>();

        undirectedGraph.AddVerticesAndEdgeRange(undirectedEdges.Select(edge => new UndirectedEdge<Vertex>(edge.Source, edge.Target)));

        _algorithm = new(undirectedGraph, minCliqueSize, maxCliqueSize);

        _algorithm.Compute();

        if (_algorithm.State == ComputationState.Aborted)
        {
            Aborted?.Invoke(this, EventArgs.Empty);

            return new List<Graph>();
        }

        var subgraphs = new List<Graph>();

        foreach (var maximumCliques in _algorithm.MaximumCliques)
        {
            var subgraph = new Graph();

            foreach (var source in maximumCliques)
            foreach (var target in maximumCliques)
            {
                if (directedEdges.TryGetValue((source, target), out var directedEdge))
                    subgraph.AddEdge(directedEdge);
            }

            subgraphs.Add(subgraph);
        }

        return subgraphs;
    }

    public void Abort() =>
        _algorithm?.Abort();

    #endregion Methods
}
