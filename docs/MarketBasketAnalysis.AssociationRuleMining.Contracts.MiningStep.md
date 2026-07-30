# <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningStep"></a> Enum MiningStep

Namespace: [MarketBasketAnalysis.AssociationRuleMining.Contracts](MarketBasketAnalysis.AssociationRuleMining.Contracts.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the steps of the association rule mining process.

```csharp
public enum MiningStep
```

## Fields

`GenerateAssociationRules = 2` 

The step where association rules are generated from the frequent itemsets.



`SearchForFrequentItems = 0` 

The step where frequent items are identified based on the minimum support threshold.



`SearchForFrequentPairs = 1` 

The step where frequent pairs are generated from the frequent items.



