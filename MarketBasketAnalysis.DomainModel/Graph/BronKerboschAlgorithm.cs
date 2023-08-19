using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Cliques;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.ContractsLight;
using System.Linq;

namespace MarketBasketAnalysis.DomainModel.Graph;

internal sealed class BronKerboschAlgorithm<TVertex, TEdge> : MaximumCliqueAlgorithmBase<TVertex, TEdge>
    where TEdge : IEdge<TVertex>
    where TVertex : notnull
{
    #region Nested types

    private sealed record Stack(TVertex[] Clique, HashSet<TVertex> CandidateVertices, HashSet<TVertex> ExcludedVertices);

    #endregion Nested types

    #region Fields and Properties

    private readonly int _minCliqueSize;
    private readonly int _maxCliqueSize;

    private readonly ConcurrentBag<IReadOnlySet<TVertex>> _maxCliques;
    private readonly Dictionary<TVertex, HashSet<TVertex>> _adjacencyList;

    public IReadOnlyCollection<IReadOnlySet<TVertex>> MaximumCliques => _maxCliques;

    #endregion Fields and Properties

    #region Constructors

    public BronKerboschAlgorithm(IUndirectedGraph<TVertex, TEdge> visitedGraph,
        int minCliqueSize, int maxCliqueSize)
        : base(visitedGraph)
    {
        Contract.RequiresNotNull(visitedGraph);
        Contract.Requires(minCliqueSize > 0);
        Contract.Requires(maxCliqueSize > 0);
        Contract.Requires(minCliqueSize <= maxCliqueSize);

        _minCliqueSize = minCliqueSize;
        _maxCliqueSize = maxCliqueSize;

        _maxCliques = new ConcurrentBag<IReadOnlySet<TVertex>>();
        _adjacencyList = new Dictionary<TVertex, HashSet<TVertex>>();
    }

    #endregion Constructors

    #region Methods

    protected override void Initialize()
    {
        _maxCliques.Clear();

        void AddAdjacencyPair(TVertex fromVertex, TVertex toVertex)
        {
            if (!_adjacencyList.TryGetValue(fromVertex, out var adjacentVertices))
            {
                adjacentVertices = new HashSet<TVertex>();

                _adjacencyList.Add(fromVertex, adjacentVertices);
            }

            adjacentVertices.Add(toVertex);
        }

        foreach (var edge in VisitedGraph.Edges)
        {
            AddAdjacencyPair(edge.Source, edge.Target);
            AddAdjacencyPair(edge.Target, edge.Source);
        }
    }

    protected override void InternalCompute()
    {
        var initialStack = new Stack(Array.Empty<TVertex>(), _adjacencyList.Keys.ToHashSet(), new HashSet<TVertex>());
        var stacks = new List<Stack> { initialStack };

        for (var i = 0; i < _maxCliqueSize; i++)
        {
            if (State == ComputationState.Aborted)
                break;

            stacks = stacks.AsParallel().Select(stack =>
            {
                var newStacks = new List<Stack>();

                if (State == ComputationState.Aborted)
                    return newStacks;

                foreach (var candidateVertex in stack.CandidateVertices)
                {
                    var newClique = stack.Clique.Append(candidateVertex).ToArray();
                    var newCandidateVertices = stack.CandidateVertices.ToHashSet();
                    var newExcludedVertices = stack.ExcludedVertices.ToHashSet();

                    newCandidateVertices.IntersectWith(_adjacencyList[candidateVertex]);
                    newExcludedVertices.IntersectWith(_adjacencyList[candidateVertex]);

                    if (newCandidateVertices.Count == 0 && newExcludedVertices.Count == 0)
                    {
                        if (newClique.Length >= _minCliqueSize)
                            _maxCliques.Add(newClique.ToHashSet());
                    }
                    else if (newCandidateVertices.Count > 0)
                    {
                        var newStack = new Stack(newClique, newCandidateVertices, newExcludedVertices);

                        newStacks.Add(newStack);
                    }

                    stack.CandidateVertices.Remove(candidateVertex);
                    stack.ExcludedVertices.Add(candidateVertex);
                }

                return newStacks;
            })
            .SelectMany(stacks => stacks)
            .ToList();
        }
    }

    protected override void Clean() =>
        _adjacencyList.Clear();

    #endregion Methods
}