# <a id="MarketBasketAnalysis_Mining_ItemExcluder"></a> Class ItemExcluder

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

```csharp
public class ItemExcluder : IItemExcluder
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[ItemExcluder](MarketBasketAnalysis.Mining.ItemExcluder.md)

#### Implements

[IItemExcluder](MarketBasketAnalysis.Mining.IItemExcluder.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.MemberwiseClone\(\)](https://learn.microsoft.com/dotnet/api/system.object.memberwiseclone), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Mining_ItemExcluder__ctor_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemExclusionRule__"></a> ItemExcluder\(IReadOnlyCollection<ItemExclusionRule\>\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.ItemExcluder" data-throw-if-not-resolved="false"></xref> class with the specified collection of exclusion rules.

```csharp
public ItemExcluder(IReadOnlyCollection<ItemExclusionRule> itemExclusionRules)
```

#### Parameters

`itemExclusionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)\>

A collection of <xref href="MarketBasketAnalysis.Mining.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for excluding items.
Each rule specifies the criteria for determining whether an item should be excluded.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">itemExclusionRules</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if <code class="paramref">itemExclusionRules</code> is empty or contains <code>null</code> items.

## Methods

### <a id="MarketBasketAnalysis_Mining_ItemExcluder_ShouldExclude_MarketBasketAnalysis_Item_"></a> ShouldExclude\(Item\)

Determines whether the specified item should be excluded.

```csharp
public bool ShouldExclude(Item item)
```

#### Parameters

`item` [Item](MarketBasketAnalysis.Item.md)

The <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to evaluate.

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

<code>true</code> if the item should be excluded; otherwise, <code>false</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">item</code> is <code>null</code>.

