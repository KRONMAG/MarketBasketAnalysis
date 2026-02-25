using MarketBasketAnalysis;
using MarketBasketAnalysis.Examples;
using MarketBasketAnalysis.Extensions;

// 1. Define items
var milk = new Item(1, "Milk");
var chicken = new Item(2, "Chicken");
var cheese = new Item(3, "Cheese");
var bread = new Item(4, "Bread");
var iceCream = new Item(5, "Ice Cream");
var lemonade = new Item(6, "Lemonade");
var salad = new Item(7, "Salad Mix");
var soup = new Item(8, "Soup");
var jacket = new Item(9, "Jacket");
var umbrella = new Item(10, "Umbrella");

// 2. Define transactions for products bought in summer
IEnumerable<Item[]> summerTransactions =
[
    [iceCream, lemonade, salad],
    [chicken, salad, lemonade],
    [milk, iceCream, lemonade],
    [bread, chicken, cheese, lemonade],
    [salad, chicken, milk],
    [iceCream, milk, bread],
    [lemonade, salad, iceCream, chicken],
    [milk, cheese, bread, iceCream, lemonade]
];

// 3. Define transactions for products bought in autumn
IEnumerable<Item[]> autumnTransactions =
[
    [soup, bread, chicken],
    [jacket, umbrella, bread, milk],
    [chicken, soup, bread, cheese],
    [milk, bread, soup, chicken],
    [jacket, umbrella, soup, bread],
    [cheese, soup, chicken, milk],
    [bread, milk, cheese, soup, chicken],
    [umbrella, jacket, milk, bread]
];

// 4. Create miner instance
var miner = ExampleHelper.CreateMiner();

// 5. Mine association rules
var summerAssociationRules = miner.Mine(
    transactions: summerTransactions,
    parameters: new (minSupport: 0.2, minConfidence: 0.5));
var autumnAssociationRules = miner.Mine(
    transactions: autumnTransactions,
    parameters: new (minSupport: 0.2, minConfidence: 0.5));

// 6. Find and print association rules that are common to both summer and autumn transactions
var commonAssociationRules = summerAssociationRules.Common(autumnAssociationRules);

Console.WriteLine("Summer association rules common to autumn:");

ExampleHelper.Print(commonAssociationRules);

// 7. Find and print association rules specific only to summer transactions
var uniqueAssociationRules = summerAssociationRules.Difference(autumnAssociationRules);

Console.WriteLine("Only summer association rules:");

ExampleHelper.Print(uniqueAssociationRules);

// Example output:

// Summer association rules common to autumn:
// Cheese -> Bread: support 0.25, confidence 1.00
// Bread -> Milk: support 0.25, confidence 0.67
// Milk -> Bread: support 0.25, confidence 0.50

// Only summer association rules:
// Ice Cream -> Milk: support 0.38, confidence 0.60
// Milk -> Ice Cream: support 0.38, confidence 0.75
// Bread -> Ice Cream: support 0.25, confidence 0.67
// Salad Mix -> Lemonade: support 0.38, confidence 0.75
// Lemonade -> Salad Mix: support 0.38, confidence 0.50
// Lemonade -> Ice Cream: support 0.50, confidence 0.67
// Ice Cream -> Lemonade: support 0.50, confidence 0.80
// Bread -> Cheese: support 0.25, confidence 0.67
// Bread -> Lemonade: support 0.25, confidence 0.67
// Lemonade -> Chicken: support 0.38, confidence 0.50
// Chicken -> Lemonade: support 0.38, confidence 0.75
// Salad Mix -> Ice Cream: support 0.25, confidence 0.50
// Salad Mix -> Chicken: support 0.38, confidence 0.75
// Chicken -> Salad Mix: support 0.38, confidence 0.75
// Milk -> Lemonade: support 0.25, confidence 0.50
// Cheese -> Lemonade: support 0.25, confidence 1.00