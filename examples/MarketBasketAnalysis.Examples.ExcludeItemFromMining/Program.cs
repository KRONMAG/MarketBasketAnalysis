using MarketBasketAnalysis;
using MarketBasketAnalysis.Examples;
using MarketBasketAnalysis.Mining;

// 1. Define transactions
var beef = new Item(1, "Beef");
var chicken = new Item(2, "Chicken");
var milk = new Item(3, "Milk");
var cheese = new Item(4, "Cheese");
var boots = new Item(5, "Boots");
var clothes = new Item(6, "Clothes");

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

// 2. Create miner instance
var miner = ExampleHelper.CreateMiner();

// 3. Define item exclusion rules
var itemExclusionRules = new List<ItemExclusionRule>
{
    new (pattern: "Beef", exactMatch: true, ignoreCase: false, applyToItems: true, applyToGroups: false),
    new (pattern: "cloth", exactMatch: false, ignoreCase: true, applyToItems: true, applyToGroups: false),
    new (pattern: "CHICKEN", exactMatch: true, ignoreCase: true, applyToItems: true, applyToGroups: false),
};

// 4. Mine association rules
var associationRules = miner.Mine(
    transactions: transactions,
    parameters: new (minSupport: 0, minConfidence: 0, itemExclusionRules: itemExclusionRules));

// 5. Output discovered association rules
ExampleHelper.Print(associationRules);

// Example output:

// Boots -> Cheese: support 0.29, confidence 1.00
// Cheese -> Boots: support 0.29, confidence 0.50
// Cheese-> Milk: support 0.14, confidence 0.25
// Milk-> Cheese: support 0.14, confidence 0.25