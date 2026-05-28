using MarketBasketAnalysis.Mining;

namespace MarketBasketAnalysis.UnitTests;

#pragma warning disable CA1515 // Consider making public types internal
public class MinerTestsAsync : MinerTestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    protected override Task<IReadOnlyCollection<AssociationRule>> MineAsync(
        IMiner miner,
        IEnumerable<Item[]> transactions,
        MiningParameters parameters,
#pragma warning disable CA1062 // Validate arguments of public methods
        CancellationToken cancellationToken = default) =>
        miner.MineAsync(transactions.ToAsyncEnumerable(), parameters, cancellationToken);
#pragma warning restore CA1062 // Validate arguments of public methods
}