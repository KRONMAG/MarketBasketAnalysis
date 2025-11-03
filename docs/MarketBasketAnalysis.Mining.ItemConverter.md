# <a id="MarketBasketAnalysis_Mining_ItemConverter"></a> Class ItemConverter

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

```csharp
public sealed class ItemConverter : IItemConverter
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[ItemConverter](MarketBasketAnalysis.Mining.ItemConverter.md)

#### Implements

[IItemConverter](MarketBasketAnalysis.Mining.IItemConverter.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Mining_ItemConverter__ctor_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemConversionRule__"></a> ItemConverter\(IReadOnlyCollection<ItemConversionRule\>\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.ItemConverter" data-throw-if-not-resolved="false"></xref> class with the specified collection of conversion rules.

```csharp
public ItemConverter(IReadOnlyCollection<ItemConversionRule> itemConversionRules)
```

#### Parameters

`itemConversionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)\>

A collection of <xref href="MarketBasketAnalysis.Mining.ItemConversionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for converting items.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">itemConversionRules</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">itemConversionRules</code> is empty or contains <code>null</code> or same rules.

## Methods

### <a id="MarketBasketAnalysis_Mining_ItemConverter_TryConvert_MarketBasketAnalysis_Item_MarketBasketAnalysis_Item__"></a> TryConvert\(Item, out Item\)

Determines whether the specified item should be replaced with a group representation.

```csharp
public bool TryConvert(Item item, out Item group)
```

#### Parameters

`item` [Item](MarketBasketAnalysis.Item.md)

The <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to evaluate.

`group` [Item](MarketBasketAnalysis.Item.md)

When this method returns, contains the group representation of the specified <code class="paramref">item</code>, 
if a replacement is required; otherwise, contains <code>null</code>.

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

<code>true</code> if the item should be replaced with a group representation; otherwise, <code>false</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">item</code> is <code>null</code>.

