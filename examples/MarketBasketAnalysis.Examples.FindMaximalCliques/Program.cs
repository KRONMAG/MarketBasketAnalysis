using MarketBasketAnalysis;
using MarketBasketAnalysis.Analysis;
using MarketBasketAnalysis.Examples;

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

// 3. Mine association rules
var associationRules = miner.Mine(transactions, parameters: new (minSupport: 0.2, minConfidence: 0.4));

// 4. Create maximal clique finder instance
var maximalCliqueFinder = ExampleHelper.CreateMaximalCliqueFinder();

// 5. Configure parameters to search maximal cliques containing from 3 to 5 vertices among symmetric association rules
var maximalCliqueFindingParameters = new MaximalCliqueFindingParameters(
    minCliqueSize: 3,
    maxCliqueSize: 5,
    ignoreOneWayLinks: true);

// 6. Search maximal cliques
var maximalCliques = maximalCliqueFinder.Find(associationRules, maximalCliqueFindingParameters);

// 7. Output found maximal cliques
ExampleHelper.Print(maximalCliques);

// Example output:

// Maximal clique 1:
// Chicken  ->  Milk
// Chicken -> Clothes
// Milk -> Chicken
// Milk -> Clothes
// Clothes -> Chicken
// Clothes -> Milk

// Maximal clique 2:
// Chicken -> Milk
// Chicken -> Beef
// Milk -> Chicken
// Milk -> Beef
// Beef -> Chicken
// Beef -> Milk

// Maximal clique 3:
// Chicken -> Cheese
// Chicken -> Beef
// Cheese -> Chicken
// Cheese -> Beef
// Beef -> Chicken
// Beef -> Cheese