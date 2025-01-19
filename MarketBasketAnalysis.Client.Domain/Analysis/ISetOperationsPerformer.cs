using System.Collections.Generic;

namespace MarketBasketAnalysis.Client.Domain.Analysis
{
    public interface ISetOperationsPerformer
    {
        IReadOnlyCollection<AssociationRule> Intersect(IReadOnlyCollection<AssociationRule> first,
            IReadOnlyCollection<AssociationRule> second, bool ignoreLinkDirection = false);

        IReadOnlyCollection<AssociationRule> Except(IReadOnlyCollection<AssociationRule> first,
            IReadOnlyCollection<AssociationRule> second, bool ignoreLinkDirection = false);
    }
}