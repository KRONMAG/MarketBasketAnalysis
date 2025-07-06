using System;
using System.Collections;
using System.Collections.Generic;

namespace MarketBasketAnalysis.Analysis
{
    internal class MaximalClique<TVertex> : IEnumerable<TVertex> where TVertex: struct
    {
        private readonly IReadOnlyCollection<TVertex> _vertices;

        public MaximalClique(IReadOnlyCollection<TVertex> vertices) =>
            _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));

        public IEnumerator<TVertex> GetEnumerator() =>
            _vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
