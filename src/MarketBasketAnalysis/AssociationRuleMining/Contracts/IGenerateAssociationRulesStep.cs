using System.Threading;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    internal interface IGenerateAssociationRulesStep
    {
        GenerateAssociationRulesResult Run(
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            SearchForItemsetsResult searchForItemsetsResult,
            MiningParameters parameters,
            CancellationToken cancellationToken);
    }
}