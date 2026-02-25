using MarketBasketAnalysis;
using MarketBasketAnalysis.Mining;
using Microsoft.Extensions.DependencyInjection;

// 1. Define items
var beef = new Item(1, "Beef");
var chicken = new Item(2, "Chicken");
var milk = new Item(3, "Milk");
var cheese = new Item(4, "Cheese");
var boots = new Item(5, "Boots");
var clothes = new Item(6, "Clothes");

// 2. Define transactions
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

// 3. Configure DI container
var services = new ServiceCollection();
services.AddMarketBasketAnalysis();
await using var serviceProvider = services.BuildServiceProvider();

// 4. Create miner instance
var minerFactory = serviceProvider.GetRequiredService<IMinerFactory>();
var miner = minerFactory.Create();

// 5. Configure mining parameters
var miningParameters = new MiningParameters(minSupport: 0.4, minConfidence: 0.5);

// 6. Mine association rules
var associationRules = miner.Mine(transactions, miningParameters);

// 7. Output discovered association rules
foreach (var associationRule in associationRules)
{
    Console.WriteLine($"{associationRule}: support {associationRule.Support:f2}, confidence {associationRule.Confidence:f2}");
}

// Example output:

// Clothes -> Chicken: support 0.43, confidence 1.00
// Chicken -> Clothes: support 0.43, confidence 0.60
// Clothes -> Milk: support 0.43, confidence 1.00
// Milk -> Clothes: support 0.43, confidence 0.75
// Milk -> Chicken: support 0.57, confidence 1.00
// Chicken -> Milk: support 0.57, confidence 0.80