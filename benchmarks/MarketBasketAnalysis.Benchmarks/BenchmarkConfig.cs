using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace MarketBasketAnalysis.Benchmarks;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithWarmupCount(1)
            .WithIterationCount(3));
        AddColumn(new AssociationRulesColumn());
    }
}
