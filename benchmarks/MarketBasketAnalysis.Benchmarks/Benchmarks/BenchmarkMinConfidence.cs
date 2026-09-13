using BenchmarkDotNet.Attributes;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Benchmark for measuring the impact of MinConfidence on mining performance.
/// Fixed parameters: MaxDegreeOfParallelism=8, StatePartitionsCount=8, MinSupport=0.01.
/// </summary>
public class BenchmarkMinConfidence : BenchmarkBase
{
    [Params(0.1, 0.3, 0.5, 0.7)]
    public double MinConfidence { get; set; }

    [Benchmark]
    public void Mine()
    {
        var parameters = CreateMiningParameters(
            minSupport: 0.01,
            minConfidence: MinConfidence,
            maxDegreeOfParallelism: 8,
            statePartitionsCount: 8);

        MineWithParameters(parameters);
    }
}
