# <a id="MarketBasketAnalysis_Mining_MiningParameters"></a> Class MiningParameters

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the parameters used for mining association rules.

```csharp
[PublicAPI]
public sealed class MiningParameters
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_Mining_MiningParameters__ctor_System_Double_System_Double_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemConversionRule__System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_Mining_ItemExclusionRule__System_Int32_"></a> MiningParameters\(double, double, IReadOnlyCollection<ItemConversionRule\>, IReadOnlyCollection<ItemExclusionRule\>, int\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.MiningParameters" data-throw-if-not-resolved="false"></xref> class.

```csharp
public MiningParameters(double minSupport, double minConfidence, IReadOnlyCollection<ItemConversionRule> itemConversionRules = null, IReadOnlyCollection<ItemExclusionRule> itemExclusionRules = null, int degreeOfParallelism = 1)
```

#### Parameters

`minSupport` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum support threshold for identifying frequent itemsets.

`minConfidence` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum confidence threshold for generating association rules.

`itemConversionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)\>

An optional collection of <xref href="MarketBasketAnalysis.Mining.ItemConversionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for converting items.

`itemExclusionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)\>

An optional collection of <xref href="MarketBasketAnalysis.Mining.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for excluding items.

`degreeOfParallelism` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The degree of parallelism to use during the mining process.

#### Exceptions

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

<p><strong>Thrown if:</strong></p><ol><li>
            <code class="paramref">minSupport</code> or <code class="paramref">minConfidence</code> is not between 0 and 1;
        </li><li>
            <code class="paramref">degreeOfParallelism</code> is not between 1 and 512.
        </li></ol>

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

<p><strong>Thrown if:</strong></p><ol><li>
            <code class="paramref">itemConversionRules</code> is empty or contains <code>null</code> or duplicates;
        </li><li>
            <code class="paramref">itemExclusionRules</code> is empty or contains <code>null</code> items.
        </li></ol>

## Properties

### <a id="MarketBasketAnalysis_Mining_MiningParameters_DegreeOfParallelism"></a> DegreeOfParallelism

Gets the degree of parallelism to use during the mining process.

```csharp
public int DegreeOfParallelism { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

#### Remarks

The value must be between 1 and 512, where higher values allow for more parallel processing.

### <a id="MarketBasketAnalysis_Mining_MiningParameters_ItemConversionRules"></a> ItemConversionRules

Gets the collection of <xref href="MarketBasketAnalysis.Mining.ItemConversionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for converting items.

```csharp
public IReadOnlyCollection<ItemConversionRule> ItemConversionRules { get; }
```

#### Property Value

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)\>

### <a id="MarketBasketAnalysis_Mining_MiningParameters_ItemExclusionRules"></a> ItemExclusionRules

Gets collection of <xref href="MarketBasketAnalysis.Mining.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for excluding items.

```csharp
public IReadOnlyCollection<ItemExclusionRule> ItemExclusionRules { get; }
```

#### Property Value

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)\>

### <a id="MarketBasketAnalysis_Mining_MiningParameters_MinConfidence"></a> MinConfidence

Gets the minimum confidence threshold for generating association rules.

```csharp
public double MinConfidence { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

#### Remarks

The value must be between 0 and 1, where 0 means no confidence is required, and 1 means the rule must always hold true.

### <a id="MarketBasketAnalysis_Mining_MiningParameters_MinSupport"></a> MinSupport

Gets the minimum support threshold for identifying frequent itemsets.

```csharp
public double MinSupport { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

#### Remarks

The value must be between 0 and 1, where 0 means no support is required, and 1 means the itemset must appear in all transactions.

