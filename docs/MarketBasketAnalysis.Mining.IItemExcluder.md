# <a id="MarketBasketAnalysis_Mining_IItemExcluder"></a> Interface IItemExcluder

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Defines a method to determine whether a specific item should be excluded during association rule mining.

```csharp
[PublicAPI]
public interface IItemExcluder
```

## Methods

### <a id="MarketBasketAnalysis_Mining_IItemExcluder_ShouldExclude_MarketBasketAnalysis_Item_"></a> ShouldExclude\(Item\)

Determines whether the specified item should be excluded.

```csharp
bool ShouldExclude(Item item)
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

