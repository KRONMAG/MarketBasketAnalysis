# <a id="MarketBasketAnalysis_Mining_IMinerFactory"></a> Interface IMinerFactory

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for factory for creating <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> instances.

```csharp
[PublicAPI]
public interface IMinerFactory
```

## Remarks

Use this interface to obtain new association rule miners.

## Methods

### <a id="MarketBasketAnalysis_Mining_IMinerFactory_Create"></a> Create\(\)

Creates a new <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> instance.

```csharp
IMiner Create()
```

#### Returns

 [IMiner](MarketBasketAnalysis.Mining.IMiner.md)

A new <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> that can be used to perform association rule mining.

