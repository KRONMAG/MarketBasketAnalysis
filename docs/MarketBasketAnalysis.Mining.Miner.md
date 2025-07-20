# <a id="MarketBasketAnalysis_Mining_Miner"></a> Class Miner

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

```csharp
public sealed class Miner : IMiner
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[Miner](MarketBasketAnalysis.Mining.Miner.md)

#### Implements

[IMiner](MarketBasketAnalysis.Mining.IMiner.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Methods

### <a id="MarketBasketAnalysis_Mining_Miner_Mine_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_Item____MarketBasketAnalysis_Mining_MiningParameters_"></a> Mine\(IEnumerable<Item\[\]\>, MiningParameters\)

Performs association rule mining synchronously.

```csharp
public IReadOnlyCollection<AssociationRule> Mine(IEnumerable<Item[]> transactions, MiningParameters parameters)
```

#### Parameters

`transactions` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[Item](MarketBasketAnalysis.Item.md)\[\]\>

A collection of transactions, where each transaction is represented as an array of items.

`parameters` [MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A collection of association rules that meet the specified parameters.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">transactions</code> or <code class="paramref">parameters</code> is <code>null</code>.

### <a id="MarketBasketAnalysis_Mining_Miner_MineAsync_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_Item____MarketBasketAnalysis_Mining_MiningParameters_System_Threading_CancellationToken_"></a> MineAsync\(IEnumerable<Item\[\]\>, MiningParameters, CancellationToken\)

Performs association rule mining asynchronously.

```csharp
public Task<IReadOnlyCollection<AssociationRule>> MineAsync(IEnumerable<Item[]> transactions, MiningParameters parameters, CancellationToken token = default)
```

#### Parameters

`transactions` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[Item](MarketBasketAnalysis.Item.md)\[\]\>

A collection of transactions, where each transaction is represented as an array of items.

`parameters` [MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

`token` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to interrupt the operation.

#### Returns

 [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>\>

A task representing the asynchronous operation, with a result of a collection of association rules.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">transactions</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">token</code>.

### <a id="MarketBasketAnalysis_Mining_Miner_MiningProgressChanged"></a> MiningProgressChanged

Event triggered when the mining progress changes.

```csharp
public event EventHandler<double> MiningProgressChanged
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[double](https://learn.microsoft.com/dotnet/api/system.double)\>

### <a id="MarketBasketAnalysis_Mining_Miner_MiningStageChanged"></a> MiningStageChanged

Event triggered when the mining stage changes.

```csharp
public event EventHandler<MiningStage> MiningStageChanged
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[MiningStage](MarketBasketAnalysis.Mining.MiningStage.md)\>

