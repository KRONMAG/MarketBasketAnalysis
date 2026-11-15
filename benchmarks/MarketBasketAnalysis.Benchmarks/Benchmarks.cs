using BenchmarkDotNet.Attributes;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
#pragma warning disable CA1724 // Type names should not match namespaces

namespace MarketBasketAnalysis.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(BenchmarkConfig))]
public class Benchmarks
{
    private const double MinConfidence = 0.01;

    private int _associationRulesCount;

    [Params(0.01, 0.001, 0.0001, 0.00001)]
    public double MinSupport { get; set; }

    [Params(1, 4, 8, 16)]
    public int MaxDegreeOfParallelism { get; set; }

    [GlobalSetup]
    public static async Task Setup()
    {
        await InstacartDatasetContext.InitializeAsync();
    }

    [Benchmark]
    public void Mine()
    {
        var miner = MinerContext.Miner;
        var transactions = InstacartDatasetContext.ReadTransactions();
        var parameters = new MiningParameters(
            MinSupport,
            MinConfidence,
            maxDegreeOfParallelism: MaxDegreeOfParallelism);

        var associationRules = miner.Mine(transactions, parameters);

        _associationRulesCount = associationRules.Count;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Console.WriteLine($"{AssociationRulesColumn.MarkerPrefix} {_associationRulesCount}");
    }
}
