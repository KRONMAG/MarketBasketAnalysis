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
var dairyProducts = new Item(7, "Dairy products", true);

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

// 4. Mine association rules without converting any item to the group
var standardAssociationRules = miner.Mine(
    transactions: transactions, 
    parameters: new(minSupport: 0.4, minConfidence: 0.5));

// 5. Mine generalized association rules with converting items "Cheese" and "Milk" to the group "Dairy products"
var generalizedAssociationRules = miner.Mine(
    transactions: transactions,
    parameters: new(
        minSupport: 0.4,
        minConfidence: 0.5,
        itemConversionRules:
        [
            new(cheese, dairyProducts),
            new(milk, dairyProducts)
        ]));

Console.WriteLine("Standard association rules:");
exampleHelper.PrintAssociationRules(standardAssociationRules);

Console.WriteLine();

Console.WriteLine("Generalized association rules:");
exampleHelper.PrintAssociationRules(generalizedAssociationRules);

// Output:
// Standard association rules:
// Clothes -> Milk: support 0,43, confidence 1,00
// Milk -> Clothes: support 0,43, confidence 0,75
// Clothes -> Chicken: support 0,43, confidence 1,00
// Chicken -> Clothes: support 0,43, confidence 0,60
// Milk -> Chicken: support 0,57, confidence 1,00
// Chicken -> Milk: support 0,57, confidence 0,80

// Generalized association rules:
// Clothes -> Dairy products: support 0,43, confidence 1,00
// Beef -> Dairy products: support 0,43, confidence 1,00
// Dairy products -> Chicken: support 0,71, confidence 0,71
// Chicken -> Dairy products: support 0,71, confidence 1,00
// Clothes -> Chicken: support 0,43, confidence 1,00
// Chicken -> Clothes: support 0,43, confidence 0,6