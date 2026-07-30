# <a id="MarketBasketAnalysis_Models_AssociationRulePart"></a> Class AssociationRulePart

Namespace: [MarketBasketAnalysis.Models](MarketBasketAnalysis.Models.md)  
Assembly: MarketBasketAnalysis.dll  

Represents a part of an association rule, either the left-hand side or the right-hand side.

```csharp
public sealed class AssociationRulePart : IEquatable<AssociationRulePart>
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[AssociationRulePart](MarketBasketAnalysis.Models.AssociationRulePart.md)

#### Implements

[IEquatable<AssociationRulePart\>](https://learn.microsoft.com/dotnet/api/system.iequatable\-1)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Properties

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Count"></a> Count

Gets the number of transactions that contain the item in this part of the rule.

```csharp
public int Count { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Id"></a> Id

Gets the unique identifier of the item associated with this part of the rule.

```csharp
public int Id { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Item"></a> Item

Gets the item associated with this part of the rule.

```csharp
public Item Item { get; }
```

#### Property Value

 [Item](MarketBasketAnalysis.Models.Item.md)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Support"></a> Support

Gets the support of the item in this part of the rule,
which is the proportion of transactions that contain the item.

```csharp
public double Support { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

## Methods

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Equals_MarketBasketAnalysis_Models_AssociationRulePart_"></a> Equals\(AssociationRulePart\)

```csharp
public bool Equals(AssociationRulePart other)
```

#### Parameters

`other` [AssociationRulePart](MarketBasketAnalysis.Models.AssociationRulePart.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_Equals_System_Object_"></a> Equals\(object\)

```csharp
public override bool Equals(object obj)
```

#### Parameters

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_GetHashCode"></a> GetHashCode\(\)

```csharp
public override int GetHashCode()
```

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Models_AssociationRulePart_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

