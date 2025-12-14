using MarketBasketAnalysis;
using MarketBasketAnalysis.Mining;
using Microsoft.Extensions.DependencyInjection;

// 1. Define transaction data
var beef = new Item(1, "Beef", false);
var chicken = new Item(2, "Chicken", false);
var milk = new Item(3, "Milk", false);
var cheese = new Item(4, "Cheese", false);
var boots = new Item(5, "Boots", false);
var clothes = new Item(6, "Clothes", false);

IEnumerable<Item[]> transactions =
[
    [beef, chicken, milk],
    [beef, cheese],
    [cheese, boots],
    [boots, chicken, cheese],
    [beef, chicken, clothes, cheese, milk],
    [clothes, chicken, milk],
    [chicken, milk, clothes],
];

// 2. Configure DI container
var services = new ServiceCollection();
services.AddMarketBasketAnalysis();

await using var serviceProvider = services.BuildServiceProvider();

// 3. Create miner instance
var minerFactory = serviceProvider.GetRequiredService<IMinerFactory>();
var miner = minerFactory.Create();

// 4. Configure mining parameters
var miningParameters = new MiningParameters(minSupport: 0.4, minConfidence: 0.5);

// 5. Mine association rules
var associationRules = miner.Mine(transactions, miningParameters);

// 6. Output discovered association rules
foreach (var associationRule in associationRules)
{
    Console.WriteLine($"{associationRule}: support {associationRule.Support:f2}, confidence {associationRule.Confidence:f2}");
}

// Output:
// Milk -> Chicken
// Chicken -> Milk
// Clothes -> Chicken
// Chicken -> Clothes
// Clothes -> Milk
// Milk -> Clothes