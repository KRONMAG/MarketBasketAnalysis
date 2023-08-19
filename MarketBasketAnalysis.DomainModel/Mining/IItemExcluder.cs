using System.Diagnostics.Contracts;

namespace MarketBasketAnalysis.DomainModel.Mining;

public interface IItemExcluder
{
    bool ShouldExclude(string item);
}
