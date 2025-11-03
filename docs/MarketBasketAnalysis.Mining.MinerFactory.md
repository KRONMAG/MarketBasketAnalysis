# <a id="MarketBasketAnalysis_Mining_MinerFactory"></a> Class MinerFactory

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

```csharp
public class MinerFactory : IMinerFactory
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MinerFactory](MarketBasketAnalysis.Mining.MinerFactory.md)

#### Implements

[IMinerFactory](MarketBasketAnalysis.Mining.IMinerFactory.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Methods

### <a id="MarketBasketAnalysis_Mining_MinerFactory_Create"></a> Create\(\)

Creates a new <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> instance.

```csharp
public IMiner Create()
```

#### Returns

 [IMiner](MarketBasketAnalysis.Mining.IMiner.md)

A new <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> that can be used to perform association rule mining.

