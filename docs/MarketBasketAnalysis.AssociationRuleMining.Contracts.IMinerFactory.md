# <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMinerFactory"></a> Interface IMinerFactory

Namespace: [MarketBasketAnalysis.AssociationRuleMining.Contracts](MarketBasketAnalysis.AssociationRuleMining.Contracts.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for factory for creating <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner" data-throw-if-not-resolved="false"></xref> instances.

```csharp
public interface IMinerFactory
```

## Remarks

Use this interface to obtain new association rule miners.

## Methods

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMinerFactory_Create"></a> Create\(\)

Creates a new <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner" data-throw-if-not-resolved="false"></xref> instance.

```csharp
IMiner Create()
```

#### Returns

 [IMiner](MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner.md)

A new <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner" data-throw-if-not-resolved="false"></xref> that can be used to perform association rule mining.

