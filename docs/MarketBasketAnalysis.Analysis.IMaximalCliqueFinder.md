# <a id="MarketBasketAnalysis_Analysis_IMaximalCliqueFinder"></a> Interface IMaximalCliqueFinder

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for finding maximal cliques in a graph of association rules.
A maximal clique is a subset of association rules where every rule is connected to every other rule,
and no additional rules can be added without breaking this property.

```csharp
public interface IMaximalCliqueFinder
```

## Methods

### <a id="MarketBasketAnalysis_Analysis_IMaximalCliqueFinder_Find_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_"></a> Find\(IReadOnlyCollection<AssociationRule\>, MaximalCliqueFindingParameters\)

Finds maximal cliques in the given collection of association rules synchronously.

```csharp
IReadOnlyCollection<IReadOnlyCollection<AssociationRule>> Find(IReadOnlyCollection<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters)
```

#### Parameters

`associationRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The collection of association rules to analyze.

`parameters` [MaximalCliqueFindingParameters](MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.md)

The parameters for finding maximal cliques.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>\>

A collection of maximal cliques, where each clique is represented as a collection of association rules.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">associationRules</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">associationRules</code> contains <code>null</code> items or duplicate items.

### <a id="MarketBasketAnalysis_Analysis_IMaximalCliqueFinder_FindAsync_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_System_Threading_CancellationToken_"></a> FindAsync\(IReadOnlyCollection<AssociationRule\>, MaximalCliqueFindingParameters, CancellationToken\)

Finds maximal cliques in the given collection of association rules asynchronously.

```csharp
Task<IReadOnlyCollection<IReadOnlyCollection<AssociationRule>>> FindAsync(IReadOnlyCollection<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters, CancellationToken token = default)
```

#### Parameters

`associationRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The collection of association rules to analyze.

`parameters` [MaximalCliqueFindingParameters](MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.md)

The parameters for finding maximal cliques.

`token` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>\>\>

A task representing the asynchronous operation, with a result of a collection of maximal cliques,
where each clique is represented as a collection of association rules.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">associationRules</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">associationRules</code> contains <code>null</code> items or duplicate items.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">token</code>.

