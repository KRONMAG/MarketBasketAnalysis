# <a id="MarketBasketAnalysis_Analysis_IMaximalCliqueAlgorithm"></a> Interface IMaximalCliqueAlgorithm

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for algorithm that find all maximal cliques in an undirected graph.

```csharp
[PublicAPI]
public interface IMaximalCliqueAlgorithm
```

## Methods

### <a id="MarketBasketAnalysis_Analysis_IMaximalCliqueAlgorithm_Find__1_System_Collections_Generic_IReadOnlyDictionary___0_System_Collections_Generic_HashSet___0___System_Int32_System_Int32_System_Threading_CancellationToken_"></a> Find<TVertex\>\(IReadOnlyDictionary<TVertex, HashSet<TVertex\>\>, int, int, CancellationToken\)

Finds all maximal cliques in a graph represented by an adjacency list.

```csharp
IEnumerable<MaximalClique<TVertex>> Find<TVertex>(IReadOnlyDictionary<TVertex, HashSet<TVertex>> adjacencyList, int minCliqueSize, int maxCliqueSize, CancellationToken token = default) where TVertex : struct
```

#### Parameters

`adjacencyList` [IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary\-2)<TVertex, [HashSet](https://learn.microsoft.com/dotnet/api/system.collections.generic.hashset\-1)<TVertex\>\>

The adjacency list representing the graph, where the key is a vertex and the value is a set of adjacent vertices.

`minCliqueSize` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The minimum size of a clique to be included in the result.

`maxCliqueSize` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The maximum size of a clique to be included in the result.

`token` [CancellationToken](https://learn.microsoft.com/dotnet/api/system.threading.cancellationtoken)

A cancellation token to cancel the operation if needed.

#### Returns

 [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[MaximalClique](MarketBasketAnalysis.Analysis.MaximalClique\-1.md)<TVertex\>\>

An enumeration of maximal cliques, each represented by a <xref href="MarketBasketAnalysis.Analysis.MaximalClique%601" data-throw-if-not-resolved="false"></xref> object.

#### Type Parameters

`TVertex` 

The type of the graph vertex.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">adjacencyList</code> is <code>null</code> or contains <code>null</code> values.

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

Thrown if <code class="paramref">minCliqueSize</code> is less than or equal to zero, or if <code class="paramref">maxCliqueSize</code> is less than <code class="paramref">minCliqueSize</code>.

 [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception)

Thrown if the operation is canceled via the <code class="paramref">token</code>.

