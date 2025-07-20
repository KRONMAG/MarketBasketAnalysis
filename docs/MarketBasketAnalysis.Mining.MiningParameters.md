# <a id="MarketBasketAnalysis_Mining_MiningParameters"></a> Class MiningParameters

Namespace: [MarketBasketAnalysis.Mining](MarketBasketAnalysis.Mining.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the parameters used for mining association rules.

```csharp
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

### <a id="MarketBasketAnalysis_Mining_MiningParameters__ctor_System_Double_System_Double_MarketBasketAnalysis_Mining_IItemConverter_MarketBasketAnalysis_Mining_IItemExcluder_System_Int32_"></a> MiningParameters\(double, double, IItemConverter, IItemExcluder, int\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.Mining.MiningParameters" data-throw-if-not-resolved="false"></xref> class.

```csharp
public MiningParameters(double minSupport, double minConfidence, IItemConverter itemConverter = null, IItemExcluder itemExcluder = null, int degreeOfParallelism = 1)
```

#### Parameters

`minSupport` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum support threshold for identifying frequent itemsets.

`minConfidence` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum confidence threshold for generating association rules.

`itemConverter` [IItemConverter](MarketBasketAnalysis.Mining.IItemConverter.md)

An optional item converter for grouping or transforming items.

`itemExcluder` [IItemExcluder](MarketBasketAnalysis.Mining.IItemExcluder.md)

An optional item excluder for filtering out specific items.

`degreeOfParallelism` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The degree of parallelism to use during the mining process.

#### Exceptions

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

Thrown if <code class="paramref">minSupport</code> or <code class="paramref">minConfidence</code> is not between 0 and 1,
or if <code class="paramref">degreeOfParallelism</code> is not between 1 and 512.

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

### <a id="MarketBasketAnalysis_Mining_MiningParameters_ItemConverter"></a> ItemConverter

Gets the item converter used to group or transform items during the mining process.

```csharp
public IItemConverter ItemConverter { get; }
```

#### Property Value

 [IItemConverter](MarketBasketAnalysis.Mining.IItemConverter.md)

#### Remarks

This is an optional parameter that allows for custom item grouping or transformation logic.

### <a id="MarketBasketAnalysis_Mining_MiningParameters_ItemExcluder"></a> ItemExcluder

Gets the item excluder used to filter out specific items from the mining process.

```csharp
public IItemExcluder ItemExcluder { get; }
```

#### Property Value

 [IItemExcluder](MarketBasketAnalysis.Mining.IItemExcluder.md)

#### Remarks

This is an optional parameter that allows for excluding items based on custom logic.

### <a id="MarketBasketAnalysis_Mining_MiningParameters_MinConfidence"></a> MinConfidence

Gets the minimum confidence threshold for generating association rules.

```csharp
public double MinConfidence { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

#### Remarks

The value must be between 0 and 1, where 0 means no confidence is required, 
and 1 means the rule must always hold true.

### <a id="MarketBasketAnalysis_Mining_MiningParameters_MinSupport"></a> MinSupport

Gets the minimum support threshold for identifying frequent itemsets.

```csharp
public double MinSupport { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

#### Remarks

The value must be between 0 and 1, where 0 means no support is required, 
and 1 means the itemset must appear in all transactions.

