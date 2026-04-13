# <a id="MarketBasketAnalysis_AssociationRule"></a> Class AssociationRule

Namespace: [MarketBasketAnalysis](MarketBasketAnalysis.md)  
Assembly: MarketBasketAnalysis.dll  

Represents an association rule - a relationship between two items in market basket analysis,
where the presence of one item (left hand side) implies the presence of another (right hand side).

```csharp
public sealed class AssociationRule : IEquatable<AssociationRule>
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[AssociationRule](MarketBasketAnalysis.AssociationRule.md)

#### Implements

[IEquatable<AssociationRule\>](https://learn.microsoft.com/dotnet/api/system.iequatable\-1)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_AssociationRule__ctor_MarketBasketAnalysis_Item_MarketBasketAnalysis_Item_System_Int32_System_Int32_System_Int32_System_Int32_"></a> AssociationRule\(Item, Item, int, int, int, int\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.AssociationRule" data-throw-if-not-resolved="false"></xref> class.

```csharp
public AssociationRule(Item lhsItem, Item rhsItem, int lhsCount, int rhsCount, int pairCount, int transactionCount)
```

#### Parameters

`lhsItem` [Item](MarketBasketAnalysis.Item.md)

The item on the left-hand side of the rule.

`rhsItem` [Item](MarketBasketAnalysis.Item.md)

The item on the right-hand side of the rule.

`lhsCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The number of transactions containing the LHS item.

`rhsCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The number of transactions containing the RHS item.

`pairCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The number of transactions containing both the LHS and RHS items.

`transactionCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The total number of transactions.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">lhsItem</code> or <code class="paramref">rhsItem</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if the LHS and RHS items are the same.

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

Thrown if any of the counts are invalid (e.g., negative or greater than the total transaction count).

## Properties

### <a id="MarketBasketAnalysis_AssociationRule_ChiSquaredTestStatistic"></a> ChiSquaredTestStatistic

Gets the Chi-squared test statistic, which measures the independence of the LHS and RHS.

```csharp
public double ChiSquaredTestStatistic { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_Confidence"></a> Confidence

Gets the confidence of the rule, which is the proportion of transactions containing the LHS that also contain the RHS.

```csharp
public double Confidence { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_Conviction"></a> Conviction

Gets the conviction of the rule, which measures the strength of the implication in the rule.

```csharp
public double Conviction { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_LeftHandSide"></a> LeftHandSide

Gets the left-hand side (LHS) of the association rule.

```csharp
public AssociationRulePart LeftHandSide { get; }
```

#### Property Value

 [AssociationRulePart](MarketBasketAnalysis.AssociationRulePart.md)

### <a id="MarketBasketAnalysis_AssociationRule_Lift"></a> Lift

Gets the lift of the rule, which measures how much more likely the RHS is to occur given the LHS, compared to its baseline probability.

```csharp
public double Lift { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_PairCount"></a> PairCount

Gets the number of transactions that contain both the LHS and RHS of the rule.

```csharp
public int PairCount { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRule_PhiCorrelationCoefficient"></a> PhiCorrelationCoefficient

Gets the Phi correlation coefficient, which measures the strength of the relationship between the LHS and RHS.

```csharp
public double PhiCorrelationCoefficient { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_RightHandSide"></a> RightHandSide

Gets the right-hand side (RHS) of the association rule.

```csharp
public AssociationRulePart RightHandSide { get; }
```

#### Property Value

 [AssociationRulePart](MarketBasketAnalysis.AssociationRulePart.md)

### <a id="MarketBasketAnalysis_AssociationRule_Support"></a> Support

Gets the support of the rule, which is the proportion of transactions that contain both the LHS and RHS.

```csharp
public double Support { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRule_TransactionCount"></a> TransactionCount

Gets the number of transactions.

```csharp
public int TransactionCount { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRule_YuleQCoefficient"></a> YuleQCoefficient

Gets the Yule's Q coefficient, which measures the association between the LHS and RHS.

```csharp
public double YuleQCoefficient { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

## Methods

### <a id="MarketBasketAnalysis_AssociationRule_Equals_MarketBasketAnalysis_AssociationRule_"></a> Equals\(AssociationRule\)

```csharp
public bool Equals(AssociationRule other)
```

#### Parameters

`other` [AssociationRule](MarketBasketAnalysis.AssociationRule.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_AssociationRule_Equals_System_Object_"></a> Equals\(object\)

```csharp
public override bool Equals(object obj)
```

#### Parameters

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_AssociationRule_GetHashCode"></a> GetHashCode\(\)

```csharp
public override int GetHashCode()
```

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRule_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

