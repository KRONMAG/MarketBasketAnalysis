using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Benchmarks.Helpers;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Base class for all benchmarks with common initialization and configuration.
/// Configuration: 1 warmup iteration, 3 target iterations, median statistics, memory diagnostics.
/// </summary>
[MemoryDiagnoser]
[MedianColumn]
[JsonExporter]
[Config(typeof(BenchmarkConfig))]
public abstract class BenchmarkBase
{
    protected IMiner Miner { get; private set; } = null!;

    protected IEnumerable<IReadOnlyList<Item>> Transactions { get; private set; } = null!;

    [GlobalSetup]
    public async Task Setup()
    {
        await InstacartDatasetContext.InitializeAsync();
        Miner = MinerContext.Miner;
        Transactions = InstacartDatasetContext.ReadTransactions().ToList();
    }

    protected static MiningParameters CreateMiningParameters(
        double minSupport,
        double minConfidence,
        int maxDegreeOfParallelism = 8,
        int statePartitionsCount = 8)
    {
        return new MiningParameters(
            minSupport,
            minConfidence,
            maxDegreeOfParallelism: maxDegreeOfParallelism,
            statePartitionsCount: statePartitionsCount);
    }

    protected void MineWithParameters(MiningParameters parameters)
    {
        Miner.Mine(Transactions, parameters);
    }
}
