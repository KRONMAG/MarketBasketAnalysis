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

## Constructors

### <a id="MarketBasketAnalysis_Mining_Miner__ctor_System_Func_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemConversionRule__MarketBasketAnalysis_Mining_IItemConverter__System_Func_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemExclusionRule__MarketBasketAnalysis_Mining_IItemExcluder__"></a> Miner\(Func<IReadOnlyCollection<ItemConversionRule\>, IItemConverter\>, Func<IReadOnlyCollection<ItemExclusionRule\>, IItemExcluder\>\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.Miner" data-throw-if-not-resolved="false"></xref> class.

```csharp
public Miner(Func<IReadOnlyCollection<ItemConversionRule>, IItemConverter> itemConverterFactory, Func<IReadOnlyCollection<ItemExclusionRule>, IItemExcluder> itemExcluderFactory)
```

#### Parameters

`itemConverterFactory` [Func](https://learn.microsoft.com/dotnet/api/system.func\-2)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)\>, [IItemConverter](MarketBasketAnalysis.Mining.IItemConverter.md)\>

A factory function that creates an <xref href="MarketBasketAnalysis.Mining.IItemConverter" data-throw-if-not-resolved="false"></xref> based on a collection of <xref href="MarketBasketAnalysis.Mining.ItemConversionRule" data-throw-if-not-resolved="false"></xref>.
This is used to define how items are grouped or replaced during mining.

`itemExcluderFactory` [Func](https://learn.microsoft.com/dotnet/api/system.func\-2)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)\>, [IItemExcluder](MarketBasketAnalysis.Mining.IItemExcluder.md)\>

A factory function that creates an <xref href="MarketBasketAnalysis.Mining.IItemExcluder" data-throw-if-not-resolved="false"></xref> based on a collection of <xref href="MarketBasketAnalysis.Mining.ItemExclusionRule" data-throw-if-not-resolved="false"></xref>.
This is used to define which items or groups should be excluded from mining.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">itemConverterFactory</code> or <code class="paramref">itemExcluderFactory</code> is <code>null</code>.

## Methods

### <a id="MarketBasketAnalysis_Mining_Miner_Mine_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_Item____MarketBasketAnalysis_Mining_MiningParameters_System_Threading_CancellationToken_"></a> Mine\(IEnumerable<Item\[\]\>, MiningParameters, CancellationToken\)

Performs association rule mining.

```csharp
public IReadOnlyCollection<AssociationRule> Mine(IEnumerable<Item[]> transactions, MiningParameters parameters, CancellationToken token = default)
```

#### Parameters

`transactions` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[Item](MarketBasketAnalysis.Item.md)\[\]\>

A collection of transactions, where each transaction is represented as an array of items.

`parameters` [MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

`token` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A collection of association rules that meet the specified parameters.

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

