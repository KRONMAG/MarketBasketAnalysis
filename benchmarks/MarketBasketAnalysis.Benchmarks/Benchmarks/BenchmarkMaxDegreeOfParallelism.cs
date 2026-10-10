using BenchmarkDotNet.Attributes;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Benchmark for measuring the impact of MaxDegreeOfParallelism on mining performance.
/// Fixed parameters: MinSupport=0.01, MinConfidence=0.1, StatePartitionsCount=1.
/// </summary>
public class BenchmarkMaxDegreeOfParallelism : BenchmarkBase
{
    [Params(1, 4, 8, 16)]
    public int MaxDegreeOfParallelism { get; set; }

    [Benchmark]
    public void Mine()
    {
        var parameters = CreateMiningParameters(
            minSupport: 0.01,
            minConfidence: 0.1,
            maxDegreeOfParallelism: MaxDegreeOfParallelism,
            statePartitionsCount: 1);

        MineWithParameters(parameters);
    }
}
