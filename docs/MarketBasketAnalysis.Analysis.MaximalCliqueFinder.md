# <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFinder"></a> Class MaximalCliqueFinder

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

```csharp
public sealed class MaximalCliqueFinder : IMaximalCliqueFinder
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MaximalCliqueFinder](MarketBasketAnalysis.Analysis.MaximalCliqueFinder.md)

#### Implements

[IMaximalCliqueFinder](MarketBasketAnalysis.Analysis.IMaximalCliqueFinder.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFinder__ctor_MarketBasketAnalysis_Analysis_IMaximalCliqueAlgorithm_"></a> MaximalCliqueFinder\(IMaximalCliqueAlgorithm\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Analysis.MaximalCliqueFinder" data-throw-if-not-resolved="false"></xref> class.

```csharp
public MaximalCliqueFinder(IMaximalCliqueAlgorithm maximalCliqueAlgorithm)
```

#### Parameters

`maximalCliqueAlgorithm` [IMaximalCliqueAlgorithm](MarketBasketAnalysis.Analysis.IMaximalCliqueAlgorithm.md)

The algorithm implementation used to find maximal cliques in the graph.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">maximalCliqueAlgorithm</code> is <code>null</code>.

## Methods

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFinder_Find_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_AssociationRule__MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_System_Threading_CancellationToken_"></a> Find\(IEnumerable<AssociationRule\>, MaximalCliqueFindingParameters, CancellationToken\)

Finds maximal cliques in the given collection of association rules.

```csharp
public IEnumerable<IReadOnlyCollection<AssociationRule>> Find(IEnumerable<AssociationRule> associationRules, MaximalCliqueFindingParameters parameters, CancellationToken token = default)
```

#### Parameters

`associationRules` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The collection of association rules to analyze.

`parameters` [MaximalCliqueFindingParameters](MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.md)

The parameters for finding maximal cliques.

`token` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>\>

A collection of maximal cliques, where each clique is represented as a collection of association rules.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">associationRules</code> or <code class="paramref">parameters</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">associationRules</code> contains <code>null</code> items or duplicate items.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">token</code>.

