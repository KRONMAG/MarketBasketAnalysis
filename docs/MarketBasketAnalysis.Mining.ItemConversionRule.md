# <a id="MarketBasketAnalysis_Mining_ItemConversionRule"></a> Class ItemConversionRule

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Represents a rule for converting one item into another.

```csharp
[PublicAPI]
public sealed class ItemConversionRule : IEquatable<ItemConversionRule>
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)

#### Implements

[IEquatable<ItemConversionRule\>](https://learn.microsoft.com/dotnet/api/system.iequatable\-1)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule__ctor_MarketBasketAnalysis_Item_MarketBasketAnalysis_Item_"></a> ItemConversionRule\(Item, Item\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.ItemConversionRule" data-throw-if-not-resolved="false"></xref> class with the specified source and target items.

```csharp
public ItemConversionRule(Item sourceItem, Item targetItem)
```

#### Parameters

`sourceItem` [Item](MarketBasketAnalysis.Item.md)

The source <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to be converted.

`targetItem` [Item](MarketBasketAnalysis.Item.md)

The target <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to which the source item will be converted.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">sourceItem</code> or <code class="paramref">targetItem</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">sourceItem</code> is a group or if <code class="paramref">targetItem</code> is not a group.

## Properties

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule_SourceItem"></a> SourceItem

Gets the source item that will be converted.

```csharp
public Item SourceItem { get; }
```

#### Property Value

 [Item](MarketBasketAnalysis.Item.md)

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule_TargetItem"></a> TargetItem

Gets the target item to which the source item will be converted.

```csharp
public Item TargetItem { get; }
```

#### Property Value

 [Item](MarketBasketAnalysis.Item.md)

## Methods

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule_Equals_System_Object_"></a> Equals\(object\)

```csharp
public override bool Equals(object obj)
```

#### Parameters

`obj` [object](https://learn.microsoft.com/dotnet/api/system.object)

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule_GetHashCode"></a> GetHashCode\(\)

```csharp
public override int GetHashCode()
```

#### Returns

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_Mining_ItemConversionRule_ToString"></a> ToString\(\)

```csharp
public override string ToString()
```

#### Returns

 [string](https://learn.microsoft.com/dotnet/api/system.string)

