# <a id="MarketBasketAnalysis_Mining_MiningStageChangedEventArgs"></a> Class MiningStageChangedEventArgs

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Provides data for the event that occurs when the mining stage changes.

```csharp
[PublicAPI]
public sealed class MiningStageChangedEventArgs : EventArgs
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[EventArgs](https://learn.microsoft.com/dotnet/api/system.eventargs) ← 
[MiningStageChangedEventArgs](MarketBasketAnalysis.Mining.MiningStageChangedEventArgs.md)

#### Inherited Members

[EventArgs.Empty](https://learn.microsoft.com/dotnet/api/system.eventargs.empty), 
[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Properties

### <a id="MarketBasketAnalysis_Mining_MiningStageChangedEventArgs_Stage"></a> Stage

Gets the current stage of the mining process.

```csharp
public MiningStage Stage { get; }
```

#### Property Value

 [MiningStage](MarketBasketAnalysis.Mining.MiningStage.md)

