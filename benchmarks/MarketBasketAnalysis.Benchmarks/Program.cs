using System.Diagnostics;
using MarketBasketAnalysis;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Benchmarks;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable S1481
#pragma warning disable SA0001

await InstacartTransactionService.DownloadDatasetAsync().ConfigureAwait(false);

var items = await InstacartTransactionService.ReadItems().ToListAsync().ConfigureAwait(false);

Console.ReadLine();

var serviceCollection = new ServiceCollection();

serviceCollection.AddMarketBasketAnalysis();

var serviceProvider = serviceCollection.BuildServiceProvider();

var minerFactory = serviceProvider.GetRequiredService<IMinerFactory>();

var miner = minerFactory.Create();

var sw = new Stopwatch();

sw.Start();

var transactions = await InstacartTransactionService.ReadTransactions(items).ToListAsync().ConfigureAwait(false);

var associationRules = miner.Mine(transactions, new(0.001, 0.01, maxDegreeOfParallelism: 16, statePartitionsCount: 1));

sw.Stop();

Console.WriteLine(associationRules.Count);

Console.WriteLine(sw.Elapsed);

Console.ReadLine();