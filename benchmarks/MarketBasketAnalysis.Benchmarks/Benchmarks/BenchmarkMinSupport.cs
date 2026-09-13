using BenchmarkDotNet.Attributes;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Benchmark for measuring the impact of MinSupport on mining performance.
/// Fixed parameters: MaxDegreeOfParallelism=8, StatePartitionsCount=8, MinConfidence=0.1.
/// </summary>
public class BenchmarkMinSupport : BenchmarkBase
{
    [Params(0.001, 0.01, 0.05, 0.1)]
    public double MinSupport { get; set; }

    [Benchmark]
    public void Mine()
    {
        var parameters = CreateMiningParameters(
            minSupport: MinSupport,
            minConfidence: 0.1,
            maxDegreeOfParallelism: 8,
            statePartitionsCount: 8);

        MineWithParameters(parameters);
    }
}
