# <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters"></a> Class MaximalCliqueFindingParameters

Namespace: [MarketBasketAnalysis.Analysis](MarketBasketAnalysis.Analysis.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the parameters used for finding maximal cliques in a graph of association rules.

```csharp
public class MaximalCliqueFindingParameters
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MaximalCliqueFindingParameters](MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters__ctor_System_Int32_System_Int32_System_Boolean_System_Predicate_MarketBasketAnalysis_AssociationRule__"></a> MaximalCliqueFindingParameters\(int, int, bool, Predicate<AssociationRule\>\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters" data-throw-if-not-resolved="false"></xref> class.

```csharp
public MaximalCliqueFindingParameters(int minCliqueSize, int maxCliqueSize, bool ignoreOneWayLinks = false, Predicate<AssociationRule> associationRuleFilter = null)
```

#### Parameters

`minCliqueSize` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The minimum size of a clique to be considered.

`maxCliqueSize` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The maximum size of a clique to be considered.

`ignoreOneWayLinks` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether one-way links should be ignored.

`associationRuleFilter` [Predicate](https://learn.microsoft.com/dotnet/api/system.predicate\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A predicate used to filter association rules before finding maximal cliques. If <code>null</code>, all rules are considered.

#### Exceptions

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

Thrown if <code class="paramref">minCliqueSize</code> is less than or equal to zero, 
or if <code class="paramref">maxCliqueSize</code> is less than <code class="paramref">minCliqueSize</code>.

## Properties

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_AssociationRuleFilter"></a> AssociationRuleFilter

Gets the predicate used to filter association rules before finding maximal cliques.
If <code>null</code>, all rules are considered.

```csharp
public Predicate<AssociationRule> AssociationRuleFilter { get; }
```

#### Property Value

 [Predicate](https://learn.microsoft.com/dotnet/api/system.predicate\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_IgnoreOneWayLinks"></a> IgnoreOneWayLinks

Gets a value indicating whether one-way links between association rules should be ignored.

```csharp
public bool IgnoreOneWayLinks { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

#### Remarks

If set to <code>true</code>, only bidirectional links will be considered when finding cliques.

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_MaxCliqueSize"></a> MaxCliqueSize

Gets the maximum size of a clique to be considered during the search.

```csharp
public int MaxCliqueSize { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

#### Remarks

The value must be greater than or equal to <xref href="MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.MinCliqueSize" data-throw-if-not-resolved="false"></xref>.

### <a id="MarketBasketAnalysis_Analysis_MaximalCliqueFindingParameters_MinCliqueSize"></a> MinCliqueSize

Gets the minimum size of a clique to be considered during the search.

```csharp
public int MinCliqueSize { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

#### Remarks

The value must be greater than zero.

