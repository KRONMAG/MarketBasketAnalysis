# <a id="MarketBasketAnalysis_Analysis_ISetOperationsPerformer"></a> Interface ISetOperationsPerformer

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

Defines an interface for performing set operations on collections of association rules.

```csharp
public interface ISetOperationsPerformer
```

## Methods

### <a id="MarketBasketAnalysis_Analysis_ISetOperationsPerformer_Except_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__System_Boolean_"></a> Except\(IReadOnlyCollection<AssociationRule\>, IReadOnlyCollection<AssociationRule\>, bool\)

Computes the difference between two collections of association rules.

```csharp
IReadOnlyCollection<AssociationRule> Except(IReadOnlyCollection<AssociationRule> first, IReadOnlyCollection<AssociationRule> second, bool ignoreLinkDirection = false)
```

#### Parameters

`first` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The first collection of association rules.

`second` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The second collection of association rules.

`ignoreLinkDirection` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the direction of links between association rules should be ignored.
If <code>true</code>, the difference will consider rules as equal regardless of their direction.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A collection of association rules that are present in <code class="paramref">first</code> but not in <code class="paramref">second</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> contains <code>null</code> items.

### <a id="MarketBasketAnalysis_Analysis_ISetOperationsPerformer_Intersect_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRule__System_Boolean_"></a> Intersect\(IReadOnlyCollection<AssociationRule\>, IReadOnlyCollection<AssociationRule\>, bool\)

Computes the intersection of two collections of association rules.

```csharp
IReadOnlyCollection<AssociationRule> Intersect(IReadOnlyCollection<AssociationRule> first, IReadOnlyCollection<AssociationRule> second, bool ignoreLinkDirection = false)
```

#### Parameters

`first` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The first collection of association rules.

`second` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The second collection of association rules.

`ignoreLinkDirection` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the direction of links between association rules should be ignored.
If <code>true</code>, the intersection will consider rules as equal regardless of their direction.

#### Returns

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A collection of association rules that are present in both <code class="paramref">first</code> and <code class="paramref">second</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> contains <code>null</code> items.

