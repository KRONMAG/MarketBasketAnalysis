using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

/// <summary>
/// Configuration for BenchmarkDotNet with optimized settings.
/// </summary>
public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(1)
            .WithIterationCount(3));
    }
}
