using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MarketBasketAnalysis.Benchmarks.Helpers;

public static class MinerContext
{
    public static IMiner Miner { get; }

    static MinerContext()
    {
        var services = new ServiceCollection();

        services.AddMarketBasketAnalysis();

        var serviceProvider = services.BuildServiceProvider();
        var minerFactory = serviceProvider.GetRequiredService<IMinerFactory>();

        Miner = minerFactory.Create();
    }
}
