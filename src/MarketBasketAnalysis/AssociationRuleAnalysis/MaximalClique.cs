using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarketBasketAnalysis.AssociationRuleAnalysis
{
    internal sealed class MaximalClique<TVertex> : IEnumerable<TVertex>
        where TVertex : struct
    {
        private readonly IReadOnlyCollection<TVertex> _vertices;

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

        public IEnumerator<TVertex> GetEnumerator() =>
            _vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
