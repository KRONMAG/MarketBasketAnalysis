# <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMiner"></a> Interface IMiner

Namespace: [MarketBasketAnalysis.AssociationRuleMining.Contracts](MarketBasketAnalysis.AssociationRuleMining.Contracts.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for performing association rule mining based on transaction data.

```csharp
public interface IMiner
```

## Methods

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMiner_Mine_System_Collections_Generic_IEnumerable_System_Collections_Generic_IReadOnlyList_MarketBasketAnalysis_Models_Item___MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_System_Threading_CancellationToken_"></a> Mine\(IEnumerable<IReadOnlyList<Item\>\>, MiningParameters, CancellationToken\)

Performs association rule mining.

```csharp
IReadOnlyCollection<AssociationRule> Mine(IEnumerable<IReadOnlyList<Item>> transactions, MiningParameters parameters, CancellationToken cancellationToken = default)
```

#### Parameters

`transactions` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[IReadOnlyList](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist\-1)<[Item](MarketBasketAnalysis.Models.Item.md)\>\>

A collection of transactions, where each transaction is represented as a collection of items.

`parameters` [MiningParameters](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

`cancellationToken` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.Models.AssociationRule.md)\>

A collection of association rules that meet the specified parameters.

#### Remarks

The enumeration of the <code class="paramref">transactions</code> may be performed multiple times.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">transactions</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">cancellationToken</code>.

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMiner_MineAsync_System_Collections_Generic_IAsyncEnumerable_System_Collections_Generic_IReadOnlyList_MarketBasketAnalysis_Models_Item___MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_System_Threading_CancellationToken_"></a> MineAsync\(IAsyncEnumerable<IReadOnlyList<Item\>\>, MiningParameters, CancellationToken\)

Performs association rule mining asynchronously.

```csharp
Task<IReadOnlyCollection<AssociationRule>> MineAsync(IAsyncEnumerable<IReadOnlyList<Item>> transactions, MiningParameters parameters, CancellationToken cancellationToken = default)
```

#### Parameters

`transactions` [IAsyncEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerable\-1)<[IReadOnlyList](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist\-1)<[Item](MarketBasketAnalysis.Models.Item.md)\>\>

A collection of transactions, where each transaction is represented as a collection of items.

`parameters` [MiningParameters](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningParameters.md)

The mining parameters, including minimum support and confidence thresholds.

`cancellationToken` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.Models.AssociationRule.md)\>\>

A task that represents the asynchronous operation.
The task result contains a collection of association rules that meet the specified parameters.

#### Remarks

The enumeration of the <code class="paramref">transactions</code> may be performed multiple times.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">transactions</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">cancellationToken</code>.

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMiner_MiningProgressChanged"></a> MiningProgressChanged

Event triggered when the mining progress changes.

```csharp
event EventHandler<MiningProgressChangedEventArgs> MiningProgressChanged
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[MiningProgressChangedEventArgs](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningProgressChangedEventArgs.md)\>

#### Remarks

The event is triggered at intervals specified by the <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningParameters.MiningProgressChangedEventInterval" data-throw-if-not-resolved="false"></xref>.

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_IMiner_MiningStepStarted"></a> MiningStepStarted

Event triggered when the one of mining steps starts.

```csharp
event EventHandler<MiningStepStartedEventArgs> MiningStepStarted
```

#### Event Type

 [EventHandler](https://learn.microsoft.com/dotnet/api/system.eventhandler\-1)<[MiningStepStartedEventArgs](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningStepStartedEventArgs.md)\>

