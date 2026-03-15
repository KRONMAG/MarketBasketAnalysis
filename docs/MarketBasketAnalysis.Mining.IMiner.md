# <a id="MarketBasketAnalysis_Mining_IMiner"></a> Interface IMiner

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for performing association rule mining based on transaction data.

```csharp
[PublicAPI]
public interface IMiner
```

## Methods

### <a id="MarketBasketAnalysis_Mining_IMiner_Mine_System_Collections_Generic_IEnumerable_System_Collections_Generic_IReadOnlyList_MarketBasketAnalysis_Item___MarketBasketAnalysis_Mining_MiningParameters_System_Threading_CancellationToken_"></a> Mine\(IEnumerable<IReadOnlyList<Item\>\>, MiningParameters, CancellationToken\)

Performs association rule mining.

```csharp
IReadOnlyCollection<AssociationRule> Mine(IEnumerable<IReadOnlyList<Item>> transactions, MiningParameters parameters, CancellationToken cancellationToken = default)
```

#### Parameters

`transactions` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[IReadOnlyList](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist\-1)<[Item](MarketBasketAnalysis.Item.md)\>\>

A collection of transactions, where each transaction is represented as an collection of items.

`parameters` [MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

`cancellationToken` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A collection of association rules that meet the specified parameters.

#### Remarks

The enumeration of the <code class="paramref">transactions</code> may be performed multiple times.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">transactions</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">cancellationToken</code>.

### <a id="MarketBasketAnalysis_Mining_IMiner_MiningProgressUpdated"></a> MiningProgressUpdated

Event triggered when the mining progress updates.

```csharp
event EventHandler<MiningProgressChangedEventArgs> MiningProgressUpdated
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[MiningProgressChangedEventArgs](MarketBasketAnalysis.Mining.MiningProgressChangedEventArgs.md)\>

#### Remarks

The event is triggered at intervals specified by the <xref href="MarketBasketAnalysis.Mining.MiningParameters.MiningProgressInterval" data-throw-if-not-resolved="false"></xref>.

### <a id="MarketBasketAnalysis_Mining_IMiner_MiningStageChanged"></a> MiningStageChanged

Event triggered when the mining stage changes.

```csharp
event EventHandler<MiningStageChangedEventArgs> MiningStageChanged
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[MiningStageChangedEventArgs](MarketBasketAnalysis.Mining.MiningStageChangedEventArgs.md)\>

