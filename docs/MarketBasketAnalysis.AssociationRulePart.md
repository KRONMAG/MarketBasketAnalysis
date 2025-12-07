# <a id="MarketBasketAnalysis_AssociationRulePart"></a> Class AssociationRulePart

Namespace: [MarketBasketAnalysis](MarketBasketAnalysis.md)  
Assembly: MarketBasketAnalysis.dll  

Represents a part of an association rule, either the left-hand side or the right-hand side.

```csharp
[PublicAPI]
public sealed class AssociationRulePart : IEquatable<AssociationRulePart>
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[AssociationRulePart](MarketBasketAnalysis.AssociationRulePart.md)

#### Implements

[IEquatable<AssociationRulePart\>](https://learn.microsoft.com/dotnet/api/system.iequatable\-1)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_AssociationRulePart__ctor_MarketBasketAnalysis_Item_System_Int32_System_Int32_"></a> AssociationRulePart\(Item, int, int\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.AssociationRulePart" data-throw-if-not-resolved="false"></xref> class.

```csharp
public AssociationRulePart(Item item, int itemCount, int transactionCount)
```

#### Parameters

`item` [Item](MarketBasketAnalysis.Item.md)

The item associated with this part of the rule.

`itemCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The number of transactions that contain the item.

`transactionCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The total number of transactions.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">item</code> is <code>null</code>.

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

Thrown if <code class="paramref">itemCount</code> is less than 1 or
if <code class="paramref">transactionCount</code> is less than <code class="paramref">itemCount</code>.

## Properties

### <a id="MarketBasketAnalysis_AssociationRulePart_Count"></a> Count

Gets the number of transactions that contain the item in this part of the rule.

```csharp
public int Count { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRulePart_Id"></a> Id

Gets the unique identifier of the item associated with this part of the rule.

```csharp
public int Id { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRulePart_Item"></a> Item

Gets the item associated with this part of the rule.

```csharp
public Item Item { get; }
```

#### Property Value

 [Item](MarketBasketAnalysis.Item.md)

### <a id="MarketBasketAnalysis_AssociationRulePart_Support"></a> Support

Gets the support of the item in this part of the rule,
which is the proportion of transactions that contain the item.

```csharp
public double Support { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

## Methods

### <a id="MarketBasketAnalysis_AssociationRulePart_Equals_MarketBasketAnalysis_AssociationRulePart_"></a> Equals\(AssociationRulePart\)

```csharp
public bool Equals(AssociationRulePart other)
```

#### Parameters

`other` [AssociationRulePart](MarketBasketAnalysis.AssociationRulePart.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_AssociationRulePart_Equals_System_Object_"></a> Equals\(object\)

```csharp
public override bool Equals(object obj)
```

#### Parameters

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_AssociationRulePart_GetHashCode"></a> GetHashCode\(\)

```csharp
public override int GetHashCode()
```

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRulePart_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

