using BenchmarkDotNet.Attributes;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;
using Microsoft.Extensions.ObjectPool;

namespace MarketBasketAnalysis.Benchmarks;

#pragma warning disable

[MemoryDiagnoser]
public class Benchmarks
{
    [GlobalSetup]
    public async Task Setup()
    {
        await InstacartDatasetContext.InitializeAsync();
    }

    

    [Benchmark]
    public void MineAssociationRules()
    {
        var miner = MinerContext.Miner;
        var transactionPool = new DefaultObjectPool<IReadOnlyList<Item>>(new TransactionPoolPolicy(), 1500);
        var transactions = InstacartDatasetContext.ReadTransactions(transactionPool);
        var parameters = new MiningParameters(0, 0, maxDegreeOfParallelism: 16, transactionPool: transactionPool);

        miner.Mine(transactions, parameters);
    }
}
