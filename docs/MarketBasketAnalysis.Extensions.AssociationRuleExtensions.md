# <a id="MarketBasketAnalysis_Extensions_AssociationRuleExtensions"></a> Class AssociationRuleExtensions

Namespace: [MarketBasketAnalysis.Extensions](MarketBasketAnalysis.Extensions.md)  
Assembly: MarketBasketAnalysis.dll  

Defines set operations on sequences of association rules.

```csharp
public static class AssociationRuleExtensions
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[AssociationRuleExtensions](MarketBasketAnalysis.Extensions.AssociationRuleExtensions.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Methods

### <a id="MarketBasketAnalysis_Extensions_AssociationRuleExtensions_Except_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_AssociationRule__System_Collections_Generic_IEnumerable_MarketBasketAnalysis_AssociationRule__System_Boolean_"></a> Except\(IEnumerable<AssociationRule\>, IEnumerable<AssociationRule\>, bool\)

Computes the difference between two sequences of association rules.

```csharp
public static IEnumerable<AssociationRule> Except(this IEnumerable<AssociationRule> first, IEnumerable<AssociationRule> second, bool ignoreLinkDirection = false)
```

#### Parameters

`first` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The first sequence of association rules.

`second` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The second sequence of association rules.

`ignoreLinkDirection` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the direction of links between association rules should be ignored.
If <code>true</code>, the difference will consider rules as equal regardless of their direction.

#### Returns

 [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A sequence of association rules that are present in <code class="paramref">first</code> but not in <code class="paramref">second</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> contains <code>null</code> items.

### <a id="MarketBasketAnalysis_Extensions_AssociationRuleExtensions_Intersect_System_Collections_Generic_IEnumerable_MarketBasketAnalysis_AssociationRule__System_Collections_Generic_IEnumerable_MarketBasketAnalysis_AssociationRule__System_Boolean_"></a> Intersect\(IEnumerable<AssociationRule\>, IEnumerable<AssociationRule\>, bool\)

Computes the intersection of two sequences of association rules.

```csharp
public static IEnumerable<AssociationRule> Intersect(this IEnumerable<AssociationRule> first, IEnumerable<AssociationRule> second, bool ignoreLinkDirection = false)
```

#### Parameters

`first` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The first sequence of association rules.

`second` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

The second sequence of association rules.

`ignoreLinkDirection` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the direction of links between association rules should be ignored.
If <code>true</code>, the intersection will consider rules as equal regardless of their direction.

#### Returns

 [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable\-1)<[AssociationRule](MarketBasketAnalysis.AssociationRule.md)\>

A sequence of association rules that are present in both <code class="paramref">first</code> and <code class="paramref">second</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">first</code> or <code class="paramref">second</code> contains <code>null</code> items.

