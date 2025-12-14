using MarketBasketAnalysis;
using MarketBasketAnalysis.Examples.Extensions;

// 1. Create example helper instance
var exampleHelper = new ExampleHelper();

// 2. Define transaction data
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

// 3. Create miner instance
var miner = exampleHelper.CreateMiner();

// 4. Mine association rules
var associationRules = miner.Mine(
    transactions: transactions,
    parameters: new(
        minSupport: 0,
        minConfidence: 0,
        itemExclusionRules:
        [
            new (pattern: "Beef", exactMatch: true, ignoreCase: false, applyToItems: true, applyToGroups: false),
            new (pattern: "cloth", exactMatch: false, ignoreCase: true, applyToItems: true, applyToGroups: false),
            new (pattern: "CHICKEN", exactMatch: true, ignoreCase: true, applyToItems: true, applyToGroups: false),
        ]));

// 5. Output discovered association rules
exampleHelper.PrintAssociationRules(associationRules);

// Output:
// Boots -> Cheese: support 0,29, confidence 1,00
// Cheese -> Boots: support 0,29, confidence 0,50
// Cheese-> Milk: support 0,14, confidence 0,25
// Milk-> Cheese: support 0,14, confidence 0,25