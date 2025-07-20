# <a id="MarketBasketAnalysis_Mining_MiningStage"></a> Enum MiningStage

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the stages of the association rule mining process.

```csharp
public enum MiningStage
```

## Fields

`AssociationRuleGeneration = 2` 

The stage where association rules are generated from the itemsets.



`FrequentItemSearch = 0` 

The stage where frequent items are identified based on the minimum support threshold.



`ItemsetSearch = 1` 

The stage where itemsets are generated from the frequent items.



