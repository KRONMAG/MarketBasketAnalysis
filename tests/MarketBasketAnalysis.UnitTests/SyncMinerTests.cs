using MarketBasketAnalysis.Mining;

namespace MarketBasketAnalysis.UnitTests;

#pragma warning disable

public class SyncMinerTests : BaseMinerTests
{
    protected override Task<IReadOnlyCollection<AssociationRule>> MineAsync(
        IMiner miner,
        IEnumerable<IReadOnlyList<Item>> transactions,
        MiningParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var associationRules = miner.Mine(transactions, parameters, cancellationToken);

        return Task.FromResult(associationRules);
    }
}