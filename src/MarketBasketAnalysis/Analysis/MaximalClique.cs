using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarketBasketAnalysis.Analysis
{
    internal class MaximalClique<TVertex> : IEnumerable<TVertex>
    {
        private readonly IReadOnlyCollection<TVertex> _vertices;

        public MaximalClique(IReadOnlyCollection<TVertex> vertices)
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if (vertices.Any(item => item == null))
                throw new ArgumentException("Collection of vertices cannot contain null items.", nameof(vertices));

            _vertices = vertices;
        }

        public IEnumerator<TVertex> GetEnumerator() =>
            _vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
