using System;
using System.Collections;
using System.Collections.Generic;

namespace MarketBasketAnalysis.Analysis
{
    /// <summary>
    /// Represents a maximal clique in a graph, defined as a set of vertices where every two distinct vertices are adjacent,
    /// and no additional vertex can be added without breaking this property.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public class MaximalClique<TVertex> : IEnumerable<TVertex> where TVertex: struct
    {
        private readonly IReadOnlyCollection<TVertex> _vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximalClique{TVertex}"/> class with the specified vertices.
        /// </summary>
        /// <param name="vertices">The collection of vertices that form the maximal clique.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="vertices"/> is <c>null</c>.
        /// </exception>
        public MaximalClique(IReadOnlyCollection<TVertex> vertices) =>
            _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));

        /// <summary>
        /// Returns an enumerator that iterates through the vertices in the clique.
        /// </summary>
        /// <returns>An enumerator for the vertices in the clique.</returns>
        public IEnumerator<TVertex> GetEnumerator() =>
            _vertices.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the vertices in the clique.
        /// </summary>
        /// <returns>An enumerator for the vertices in the clique.</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
