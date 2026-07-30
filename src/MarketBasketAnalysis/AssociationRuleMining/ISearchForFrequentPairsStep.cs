using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface ISearchForFrequentPairsStep
    {
        SearchForFrequentPairsResult Run(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken);

        Task<SearchForFrequentPairsResult> RunAsync(
            IAsyncEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken);
    }
}