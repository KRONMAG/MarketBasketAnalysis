using System;
using System.Collections.Generic;
using System.Threading;

namespace MarketBasketAnalysis.Analysis
{
    /// <summary>
    /// Defines an interface for algorithm that find all maximal cliques in an undirected graph.
    /// </summary>
    public interface IMaximalCliqueAlgorithm
    {
        /// <summary>
        /// Finds all maximal cliques in a graph represented by an adjacency list.
        /// </summary>
        /// <typeparam name="TVertex">The type of the graph vertex.</typeparam>
        /// <param name="adjacencyList">
        /// The adjacency list representing the graph, where the key is a vertex and the value is a set of adjacent vertices.
        /// </param>
        /// <param name="minCliqueSize">
        /// The minimum size of a clique to be included in the result.
        /// </param>
        /// <param name="maxCliqueSize">
        /// The maximum size of a clique to be included in the result.
        /// </param>
        /// <param name="token">
        /// A cancellation token to cancel the operation if needed.
        /// </param>
        /// <returns>
        /// An enumeration of maximal cliques, each represented by a <see cref="MaximalClique{TVertex}"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="adjacencyList"/> is <c>null</c> or contains <c>null</c> values.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="minCliqueSize"/> is less than or equal to zero, or if <paramref name="maxCliqueSize"/> is less than <paramref name="minCliqueSize"/>.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled via the <paramref name="token"/>.
        /// </exception>
        IEnumerable<MaximalClique<TVertex>> Find<TVertex>(
            IReadOnlyDictionary<TVertex, HashSet<TVertex>> adjacencyList,
            int minCliqueSize,
            int maxCliqueSize,
            CancellationToken token = default)
            where TVertex : struct;
    }
}