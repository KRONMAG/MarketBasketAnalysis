using System.Collections.Generic;
using System.Threading;

namespace MarketBasketAnalysis.AssociationRuleAnalysis
{
    internal interface IMaximalCliqueAlgorithm
    {
        IEnumerable<MaximalClique<TVertex>> Find<TVertex>(
            IReadOnlyDictionary<TVertex, HashSet<TVertex>> adjacencyList,
            int minCliqueSize,
            int maxCliqueSize,
            CancellationToken token = default)
            where TVertex : struct;
    }
}