# <a id="MarketBasketAnalysis_Mining_IItemConverter"></a> Interface IItemConverter

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Defines a method for determining whether an item should be replaced with its corresponding group.

```csharp
[PublicAPI]
public interface IItemConverter
```

## Methods

### <a id="MarketBasketAnalysis_Mining_IItemConverter_TryConvert_MarketBasketAnalysis_Item_MarketBasketAnalysis_Item__"></a> TryConvert\(Item, out Item\)

Determines whether the specified item should be replaced with a group.

```csharp
bool TryConvert(Item item, out Item group)
```

#### Parameters

`item` [Item](MarketBasketAnalysis.Item.md)

The <xref href="MarketBasketAnalysis.Item" data-throw-if-not-resolved="false"></xref> to evaluate.

`group` [Item](MarketBasketAnalysis.Item.md)

When this method returns, contains the group of the specified <code class="paramref">item</code>,
if a replacement is required; otherwise, contains <code>null</code>.

#### Returns

 [bool](https://learn.microsoft.com/dotnet/api/system.boolean)

<code>true</code> if the item should be replaced with a group representation; otherwise, <code>false</code>.

#### Exceptions

 [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Thrown if <code class="paramref">item</code> is <code>null</code>.

