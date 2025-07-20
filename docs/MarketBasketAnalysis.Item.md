# <a id="MarketBasketAnalysis_Item"></a> Class Item

Namespace: [MarketBasketAnalysis](MarketBasketAnalysis.md)  
Assembly: MarketBasketAnalysis.dll  

Represents an item in a transaction.

```csharp
public sealed class Item : IEquatable<Item>
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[Item](MarketBasketAnalysis.Item.md)

#### Implements

[IEquatable<Item\>](https://learn.microsoft.com/dotnet/api/system.iequatable\-1)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Item__ctor_System_Int32_System_String_System_Boolean_"></a> Item\(int, string, bool\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> class.

```csharp
public Item(int id, string name, bool isGroup)
```

#### Parameters

`id` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The unique identifier of the item.

`name` [string](https://learn.microsoft.com/dotnet/api/system.string)

The name of the item.

`isGroup` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the item is a group of other items.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">name</code> is <code>null</code>.

## Properties

### <a id="MarketBasketAnalysis_Item_Id"></a> Id

Gets the unique identifier of the item.

```csharp
public int Id { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Item_IsGroup"></a> IsGroup

Gets a value indicating whether the item is a group of other items.

```csharp
public bool IsGroup { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Item_Name"></a> Name

Gets the name of the item.

```csharp
public string Name { get; }
```

#### Property Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

## Methods

### <a id="MarketBasketAnalysis_Item_Equals_MarketBasketAnalysis_Item_"></a> Equals\(Item\)

```csharp
public bool Equals(Item other)
```

#### Parameters

`other` [Item](MarketBasketAnalysis.Item.md)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Item_Equals_System_Object_"></a> Equals\(object\)

```csharp
public override bool Equals(object obj)
```

#### Parameters

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Item_GetHashCode"></a> GetHashCode\(\)

```csharp
public override int GetHashCode()
```

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Item_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

