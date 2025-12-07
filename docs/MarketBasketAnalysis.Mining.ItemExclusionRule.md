# <a id="MarketBasketAnalysis_Mining_ItemExclusionRule"></a> Class ItemExclusionRule

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Represents a rule for excluding items or groups from association rule mining.

```csharp
[PublicAPI]
public sealed class ItemExclusionRule
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule__ctor_System_String_System_Boolean_System_Boolean_System_Boolean_System_Boolean_"></a> ItemExclusionRule\(string, bool, bool, bool, bool\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> class with the specified parameters.

```csharp
public ItemExclusionRule(string pattern, bool exactMatch, bool ignoreCase, bool applyToItems, bool applyToGroups)
```

#### Parameters

`pattern` [string](https://learn.microsoft.com/dotnet/api/system.string)

The pattern used to match item names for exclusion.

`exactMatch` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the exclusion rule requires an exact match of the item name.

`ignoreCase` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the comparison should ignore case when matching item names.

`applyToItems` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the rule applies to individual items.

`applyToGroups` [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

A value indicating whether the rule applies to groups of items.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">pattern</code> is <code>null</code>.

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Thrown if both <code class="paramref">applyToItems</code> and <code class="paramref">applyToGroups</code> are <code>false</code>.

## Properties

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_ApplyToGroups"></a> ApplyToGroups

Gets a value indicating whether the rule applies to groups of items.

```csharp
public bool ApplyToGroups { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_ApplyToItems"></a> ApplyToItems

Gets a value indicating whether the rule applies to individual items.

```csharp
public bool ApplyToItems { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_ExactMatch"></a> ExactMatch

Gets a value indicating whether the exclusion rule requires an exact match of the item name.

```csharp
public bool ExactMatch { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_IgnoreCase"></a> IgnoreCase

Gets a value indicating whether the comparison should ignore case when matching item names.

```csharp
public bool IgnoreCase { get; }
```

#### Property Value

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_Pattern"></a> Pattern

Gets the pattern used to match item names for exclusion.

```csharp
public string Pattern { get; }
```

#### Property Value

 [string](https://learn.microsoft.com/dotnet/api/system.string)

## Methods

### <a id="MarketBasketAnalysis_Mining_ItemExclusionRule_ShouldExclude_MarketBasketAnalysis_Item_"></a> ShouldExclude\(Item\)

Determines whether the specified item should be excluded based on the rule.

```csharp
public bool ShouldExclude(Item item)
```

#### Parameters

`item` [Item](MarketBasketAnalysis.Item.md)

The <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to evaluate.

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

<code>true</code> if the item matches the exclusion rule and should be excluded; otherwise, <code>false</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">item</code> is <code>null</code>.

