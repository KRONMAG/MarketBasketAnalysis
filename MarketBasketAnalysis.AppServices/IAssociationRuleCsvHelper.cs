using MarketBasketAnalysis.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.AppServices;

public interface IAssociationRuleCsvHelper
{
    Task<IReadOnlyCollection<AssociationRule>> ReadAsync(string path);

    Task WriteAsync(string path, IReadOnlyCollection<AssociationRule> associationRules);
}