# <a id="MarketBasketAnalysis_Analysis_MaximalClique_1"></a> Class MaximalClique<TVertex\>

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

Represents a maximal clique in a graph, defined as a set of vertices where every two distinct vertices are adjacent,
and no additional vertex can be added without breaking this property.

```csharp
[PublicAPI]
public sealed class MaximalClique<TVertex> : IEnumerable<TVertex>, IEnumerable where TVertex : struct
```

#### Type Parameters

`TVertex` 

The type of the vertex.

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MaximalClique<TVertex\>](MarketBasketAnalysis.Analysis.MaximalClique\-1.md)

#### Implements

[IEnumerable<TVertex\>](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1), 
[IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.ienumerable)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Analysis_MaximalClique_1__ctor_System_Collections_Generic_IReadOnlyCollection__0__"></a> MaximalClique\(IReadOnlyCollection<TVertex\>\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Analysis.MaximalClique%601" data-throw-if-not-resolved="false"></xref> class with the specified vertices.

```csharp
public MaximalClique(IReadOnlyCollection<TVertex> vertices)
```

#### Parameters

`vertices` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<TVertex\>

The collection of vertices that form the maximal clique.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">vertices</code> is <code>null</code>, empty or contains duplicates.

## Methods

### <a id="MarketBasketAnalysis_Analysis_MaximalClique_1_GetEnumerator"></a> GetEnumerator\(\)

Returns an enumerator that iterates through the vertices in the clique.

```csharp
public IEnumerator<TVertex> GetEnumerator()
```

#### Returns

 [IEnumerator](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerator\-1)<TVertex\>

An enumerator for the vertices in the clique.

