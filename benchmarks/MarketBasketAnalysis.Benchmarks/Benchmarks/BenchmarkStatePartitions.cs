using BenchmarkDotNet.Attributes;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Benchmark for measuring the impact of StatePartitionsCount on mining performance.
/// Fixed parameters: MaxDegreeOfParallelism=8, MinSupport=0.01, MinConfidence=0.1.
/// </summary>
public class BenchmarkStatePartitions : BenchmarkBase
{
    [Params(1, 4, 8, 16)]
    public int StatePartitionsCount { get; set; }

    [Benchmark]
    public void Mine()
    {
        var parameters = CreateMiningParameters(
            minSupport: 0.01,
            minConfidence: 0.1,
            maxDegreeOfParallelism: 8,
            statePartitionsCount: StatePartitionsCount);

        MineWithParameters(parameters);
    }
}
