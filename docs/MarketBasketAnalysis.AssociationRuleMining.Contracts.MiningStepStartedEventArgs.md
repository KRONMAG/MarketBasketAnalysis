# <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningStepStartedEventArgs"></a> Class MiningStepStartedEventArgs

Namespace: [MarketBasketAnalysis.AssociationRuleMining.Contracts](MarketBasketAnalysis.AssociationRuleMining.Contracts.md)  
Assembly: MarketBasketAnalysis.dll  

Provides data for the event triggered when one of mining steps starts.

```csharp
public sealed class MiningStepStartedEventArgs : EventArgs
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[EventArgs](https://learn.microsoft.com/dotnet/api/system.eventargs) ← 
[MiningStepStartedEventArgs](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningStepStartedEventArgs.md)

#### Inherited Members

[EventArgs.Empty](https://learn.microsoft.com/dotnet/api/system.eventargs.empty), 
[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Properties

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningStepStartedEventArgs_Step"></a> Step

Gets the mining step that has started.

```csharp
public MiningStep Step { get; }
```

#### Property Value

 [MiningStep](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningStep.md)

