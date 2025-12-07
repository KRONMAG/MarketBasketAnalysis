using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace MarketBasketAnalysis.Analysis
{
    /// <summary>
    /// Represents a maximal clique in a graph, defined as a set of vertices where every two distinct vertices are adjacent,
    /// and no additional vertex can be added without breaking this property.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    [PublicAPI]
    public sealed class MaximalClique<TVertex> : IEnumerable<TVertex>
        where TVertex : struct
    {
        private readonly IReadOnlyCollection<TVertex> _vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximalClique{TVertex}"/> class with the specified vertices.
        /// </summary>
        /// <param name="vertices">The collection of vertices that form the maximal clique.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="vertices"/> is <c>null</c>, empty or contains duplicates.
        /// </exception>
        public MaximalClique(IReadOnlyCollection<TVertex> vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            if (vertices.Count == 0)
            {
                throw new ArgumentException("Clique cannot be empty.", nameof(vertices));
            }

            if (vertices.Distinct().Count() != vertices.Count)
            {
                throw new ArgumentException("Clique cannot contain duplicate vertices.", nameof(vertices));
            }

            _vertices = vertices;
        }

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
