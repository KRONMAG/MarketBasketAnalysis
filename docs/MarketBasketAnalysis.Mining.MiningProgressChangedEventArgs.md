# <a id="MarketBasketAnalysis_Mining_MiningProgressChangedEventArgs"></a> Class MiningProgressChangedEventArgs

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Provides data for the event that occurs when mining progress changes.

```csharp
[PublicAPI]
public sealed class MiningProgressChangedEventArgs : EventArgs
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[EventArgs](https://learn.microsoft.com/dotnet/api/system.eventargs) ← 
[MiningProgressChangedEventArgs](MarketBasketAnalysis.Mining.MiningProgressChangedEventArgs.md)

#### Inherited Members

[EventArgs.Empty](https://learn.microsoft.com/dotnet/api/system.eventargs.empty), 
[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Properties

### <a id="MarketBasketAnalysis_Mining_MiningProgressChangedEventArgs_Progress"></a> Progress

Gets the current progress of the mining operation as a value between 0 and 100.

```csharp
public double Progress { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

