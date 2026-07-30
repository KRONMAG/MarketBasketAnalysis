using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface ISearchForFrequentItemsStep
    {
        SearchForFrequentItemsResult Run(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            CancellationToken cancellationToken);

        Task<SearchForFrequentItemsResult> RunAsync(
            IAsyncEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            CancellationToken cancellationToken);
    }
}