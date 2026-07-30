using System.Threading;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface IGenerateAssociationRulesStep
    {
        GenerateAssociationRulesResult Run(
            SearchForFrequentItemsResult searchForFrequentItemsResult,
            SearchForFrequentPairsResult searchForFrequentPairsResult,
            MiningParameters parameters,
            CancellationToken cancellationToken);
    }
}