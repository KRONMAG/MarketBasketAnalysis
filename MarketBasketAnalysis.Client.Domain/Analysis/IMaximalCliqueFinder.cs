using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Client.Domain.Analysis
{
    public interface IMaximalCliqueFinder
    {
        IReadOnlyCollection<IReadOnlyCollection<AssociationRule>> Find(
            IReadOnlyCollection<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters);

        Task<IReadOnlyCollection<IReadOnlyCollection<AssociationRule>>> FindAsync(
            IReadOnlyCollection<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters,
            CancellationToken token = default);
    }
}
