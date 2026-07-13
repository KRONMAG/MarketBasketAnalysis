using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    internal interface ISearchForItemsetsStep
    {
        SearchForItemsetsResult Run(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken);

        Task<SearchForItemsetsResult> RunAsync(
            IAsyncEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            IMiningProgressChangedEventPublisher miningProgressChangedEventPublisher,
            CancellationToken cancellationToken);
    }
}