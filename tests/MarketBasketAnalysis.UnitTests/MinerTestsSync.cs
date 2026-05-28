using MarketBasketAnalysis.Mining;

namespace MarketBasketAnalysis.UnitTests;

#pragma warning disable CA1515 // Consider making public types internal
public class MinerTestsSync : MinerTestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    protected override Task<IReadOnlyCollection<AssociationRule>> MineAsync(
        IMiner miner,
        IEnumerable<Item[]> transactions,
        MiningParameters parameters,
        CancellationToken cancellationToken = default)
    {
#pragma warning disable CA1062 // Validate arguments of public methods
        var associationRules = miner.Mine(transactions, parameters, cancellationToken);
#pragma warning restore CA1062 // Validate arguments of public methods

        return Task.FromResult(associationRules);
    }
}