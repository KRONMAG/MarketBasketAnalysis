using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace MarketBasketAnalysis.Analysis
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public sealed class MaximalCliqueFinder : IMaximalCliqueFinder
    {
        private readonly IMaximalCliqueAlgorithm _maximalCliqueAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximalCliqueFinder"/> class.
        /// </summary>
        /// <param name="maximalCliqueAlgorithm">
        /// The algorithm implementation used to find maximal cliques in the graph.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="maximalCliqueAlgorithm"/> is <c>null</c>.
        /// </exception>
        public MaximalCliqueFinder(IMaximalCliqueAlgorithm maximalCliqueAlgorithm)
        {
            _maximalCliqueAlgorithm =
                maximalCliqueAlgorithm ?? throw new ArgumentNullException(nameof(maximalCliqueAlgorithm));
        }

        /// <inheritdoc />
        public IEnumerable<IReadOnlyCollection<AssociationRule>> Find(
            IEnumerable<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters,
            CancellationToken token = default)
        {
            if (associationRules == null)
                throw new ArgumentNullException(nameof(associationRules));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (associationRules.Contains(null))
            {
                throw new ArgumentException("Collection of association rules cannot contain null items.",
                    nameof(associationRules));
            }

            if (associationRules.Distinct().Count() != associationRules.Count())
            {
                throw new ArgumentException("Collection of association rules cannot contain same items.",
                    nameof(associationRules));
            }

            token.ThrowIfCancellationRequested();

            if (!associationRules.Any())
                return Array.Empty<IReadOnlyCollection<AssociationRule>>();

            var itemsCount = associationRules.Select(r => r.LeftHandSide.Item)
                .Union(associationRules.Select(r => r.RightHandSide.Item))
                .Count();

            token.ThrowIfCancellationRequested();

            if (itemsCount <= byte.MaxValue + 1)
            {
                byte vertexCounter = 0;

                var (itemToVertexMap, vertexToItemMap) = MarkupItems(associationRules, () => vertexCounter++, token);

                return FindInternal(associationRules, parameters, itemToVertexMap, vertexToItemMap, token);
            }
            // ReSharper disable once RedundantIfElseBlock
            else if (itemsCount <= ushort.MaxValue + 1)
            {
                ushort vertexCounter = 0;

                var (itemToVertexMap, vertexToItemMap) = MarkupItems(associationRules, () => vertexCounter++, token);

                return FindInternal(associationRules, parameters, itemToVertexMap, vertexToItemMap, token);
            }
            else
            {
                int vertexCounter = 0;

                var (itemToVertexMap, vertexToItemMap) = MarkupItems(associationRules, () => vertexCounter++, token);

                return FindInternal(associationRules, parameters, itemToVertexMap, vertexToItemMap, token);
            }
        }

        private static (IReadOnlyDictionary<Item, TVertex>, IReadOnlyDictionary<TVertex, Item>) MarkupItems<TVertex>(
            IEnumerable<AssociationRule> associationRules,
            Func<TVertex> generateVertex,
            CancellationToken token)
        {
            var itemToVertexMap = new Dictionary<Item, TVertex>();
            var vertexToItemMap = new Dictionary<TVertex, Item>();

            foreach (var associationRule in associationRules)
            {
                token.ThrowIfCancellationRequested();

                MarkupItem(associationRule.LeftHandSide);
                MarkupItem(associationRule.RightHandSide);
            }

            return (itemToVertexMap, vertexToItemMap);

            void MarkupItem(AssociationRulePart part)
            {
                var item = part.Item;
                
                if (itemToVertexMap.ContainsKey(item))
                    return;

                var vertex = generateVertex();

                itemToVertexMap.Add(item, vertex);
                vertexToItemMap.Add(vertex, item);
            }
        }

        private IEnumerable<IReadOnlyCollection<AssociationRule>> FindInternal<TVertex>(
            IEnumerable<AssociationRule> associationRules,
            MaximalCliqueFindingParameters parameters,
            IReadOnlyDictionary<Item, TVertex> itemToVertexMap,
            IReadOnlyDictionary<TVertex, Item> vertexToItemMap,
            CancellationToken token)
            where TVertex : struct
        {
            var adjacencyList = ConvertToAdjacencyList(associationRules, parameters, itemToVertexMap, token);
            var itemPairToAssociationRuleMap = associationRules.ToDictionary(associationRule =>
                (associationRule.LeftHandSide.Item, associationRule.RightHandSide.Item));

            token.ThrowIfCancellationRequested();

            var maximalCliques = _maximalCliqueAlgorithm.Find(
                adjacencyList, parameters.MinCliqueSize, parameters.MaxCliqueSize, token);

            foreach (var maximalClique in maximalCliques)
            {
                token.ThrowIfCancellationRequested();

                yield return ConvertToAssociationRules(maximalClique, vertexToItemMap, itemPairToAssociationRuleMap);
            }
        }

        private Dictionary<TVertex, HashSet<TVertex>> ConvertToAdjacencyList<TVertex>(
            IEnumerable<AssociationRule> associationRules,
            MaximalCliqueFindingParameters parameters,
            IReadOnlyDictionary<Item, TVertex> itemToVertexMap,
            CancellationToken token)
        {
            var adjacencyList = new Dictionary<TVertex, HashSet<TVertex>>();

            if (parameters.IgnoreOneWayLinks)
            {
                var candidateEdges = new HashSet<(TVertex, TVertex)>();

                foreach (var associationRule in associationRules)
                {
                    token.ThrowIfCancellationRequested();

                    var sourceVertex = itemToVertexMap[associationRule.LeftHandSide.Item];
                    var targetVertex = itemToVertexMap[associationRule.RightHandSide.Item];

                    if (candidateEdges.Contains((targetVertex, sourceVertex)))
                    {
                        AddAdjacencyPair(sourceVertex, targetVertex);
                        AddAdjacencyPair(targetVertex, sourceVertex);
                    }
                    else
                    {
                        candidateEdges.Add((sourceVertex, targetVertex));
                    }
                }
            }
            else
            {
                foreach (var associationRule in associationRules)
                {
                    token.ThrowIfCancellationRequested();

                    var sourceVertex = itemToVertexMap[associationRule.LeftHandSide.Item];
                    var targetVertex = itemToVertexMap[associationRule.RightHandSide.Item];

#pragma warning disable S4158 // Empty collections should not be accessed or iterated
                    if (adjacencyList.TryGetValue(sourceVertex, out var adjacentVertices) &&
                        adjacentVertices.Contains(targetVertex))
                        continue;
#pragma warning restore S4158

                    AddAdjacencyPair(sourceVertex, targetVertex);
                    AddAdjacencyPair(targetVertex, sourceVertex);
                }
            }

            return adjacencyList;

            void AddAdjacencyPair(TVertex vertex, TVertex adjacentVertex)
            {
                if (!adjacencyList.TryGetValue(vertex, out var adjacentVertices))
                {
                    adjacentVertices = new HashSet<TVertex>();

                    adjacencyList.Add(vertex, adjacentVertices);
                }

                adjacentVertices.Add(adjacentVertex);
            }
        }

        private static IReadOnlyCollection<AssociationRule> ConvertToAssociationRules<TVertex>(
            MaximalClique<TVertex> maximalClique,
            IReadOnlyDictionary<TVertex, Item> vertexToItemMap,
            IReadOnlyDictionary<(Item, Item), AssociationRule> itemPairToAssociationRuleMap)
            where TVertex : struct
        {
            var associationRuleSubset = new List<AssociationRule>();

            foreach (var sourceVertex in maximalClique)
            {
                var leftHandSide = vertexToItemMap[sourceVertex];

                foreach (var targetVertex in maximalClique)
                {
                    var rightHandSide = vertexToItemMap[targetVertex];

                    if (itemPairToAssociationRuleMap.TryGetValue((leftHandSide, rightHandSide), out var associationRule))
                        associationRuleSubset.Add(associationRule);
                }
            }

            return associationRuleSubset;
        }
    }
}
