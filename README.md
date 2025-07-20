<div style="display: flex; align-items: center">
  <img src="docs/icons/logo.png" style="float: left; margin-right: 15px"/>
  <h1>Market Basket Analysis</h1>
</div>

Library for discovering and analyzing association rules from transactional data

## Features

- Association Rule Mining: discover rules based on user-defined support and confidence thresholds
- Rule Quality Metrics: support, confidence, lift, conviction, Yule's Q coefficient, φ (phi) correlation coefficient, χ² (chi-squared) test statistic
- Pattern-Based Item Exclusion: dynamically exclude specific items from the mining process using item exlusion rules;
- Generalized Rule Mining: lift rules to higher abstraction levels using item conversion rules
- Graph Analysis: detect maximal cliques of user-specified size in the association rule graph
- Rule Set Operations: compute intersection and difference between two rule sets

## Prerequisites

- .NET Standard 2.0

## Installation

Package Manager

```powershell
Install-Package MarketBasketAnalysis
```

.NET CLI

```powershell
dotnet add package MarketBasketAnalysis
```

## Quick Start

```csharp
using MarketBasketAnalysis;
using MarketBasketAnalysis.Mining;

var beef = new Item(1, "Beef", false);
var chicken = new Item(2, "Chicken", false);
var milk = new Item(3, "Milk", false);
var cheese = new Item(4, "Cheese", false);
var boots = new Item(5, "Boots", false);
var clothes = new Item(6, "Clothes", false);

var miner = new Miner();

var associationRules = miner.Mine(
    transactions:
    [
        [beef, chicken, milk],
        [beef, cheese],
        [cheese, boots],
        [boots, chicken, cheese],
        [beef, chicken, clothes, cheese, milk],
        [clothes, chicken, milk],
        [chicken, milk, clothes],
    ],
    parameters: new(minSupport: 0.4, minConfidence: 0.5));

// Found association rules:
// Milk -> Chicken
// Chicken -> Milk
// Clothes -> Chicken
// Chicken -> Clothes
// Clothes -> Milk
// Milk -> Clothes
```

## Documentation

| Namespace | URL |
| ----------- | ----------- |
| MarketBasketAnalysis | https://github.com/KRONMAG/MarketBasketAnalysis/blob/main/docs/MarketBasketAnalysis.md |
| MarketBasketAnalysis.Mining | https://github.com/KRONMAG/MarketBasketAnalysis/blob/main/docs/MarketBasketAnalysis.Mining.md |
| MarketBasketAnalysis.Analysis | https://github.com/KRONMAG/MarketBasketAnalysis/blob/main/docs/MarketBasketAnalysis.Analysis.md |

## License

MIT License - See [LICENSE](https://github.com/KRONMAG/MarketBasketAnalysis/blob/main/LICENSE)