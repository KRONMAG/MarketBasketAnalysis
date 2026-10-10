using BenchmarkDotNet.Running;
using MarketBasketAnalysis.Benchmarks.Benchmarks;

var benchmarks = new[]
{
    typeof(BenchmarkMinSupport),
    typeof(BenchmarkMinConfidence),
    typeof(BenchmarkMaxDegreeOfParallelism),
    typeof(BenchmarkStatePartitions),
};

BenchmarkRunner.Run(benchmarks);