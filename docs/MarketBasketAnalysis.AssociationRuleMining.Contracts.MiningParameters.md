# <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters"></a> Class MiningParameters

Namespace: [MarketBasketAnalysis.AssociationRuleMining.Contracts](MarketBasketAnalysis.AssociationRuleMining.Contracts.md)  
Assembly: MarketBasketAnalysis.dll  

Represents the parameters used for mining association rules.

```csharp
public sealed class MiningParameters
```

#### Inheritance

[object](https://learn.microsoft.com/dotnet/api/system.object) ← 
[MiningParameters](MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningParameters.md)

#### Inherited Members

[object.Equals\(object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\)), 
[object.Equals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.equals\#system\-object\-equals\(system\-object\-system\-object\)), 
[object.GetHashCode\(\)](https://learn.microsoft.com/dotnet/api/system.object.gethashcode), 
[object.GetType\(\)](https://learn.microsoft.com/dotnet/api/system.object.gettype), 
[object.ReferenceEquals\(object, object\)](https://learn.microsoft.com/dotnet/api/system.object.referenceequals), 
[object.ToString\(\)](https://learn.microsoft.com/dotnet/api/system.object.tostring)

## Constructors

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters__ctor_System_Double_System_Double_System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRuleMining_Contracts_ItemConversionRule__System_Collections_Generic_IReadOnlyCollection_MarketBasketAnalysis_AssociationRuleMining_Contracts_ItemExclusionRule__System_Int32_System_Int32_System_Int32_"></a> MiningParameters\(double, double, IReadOnlyCollection<ItemConversionRule\>, IReadOnlyCollection<ItemExclusionRule\>, int, int, int\)

Initializes a new instance of the <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.MiningParameters" data-throw-if-not-resolved="false"></xref> class.

```csharp
public MiningParameters(double minSupport, double minConfidence, IReadOnlyCollection<ItemConversionRule> itemConversionRules = null, IReadOnlyCollection<ItemExclusionRule> itemExclusionRules = null, int maxDegreeOfParallelism = 1, int statePartitionsCount = 1, int miningProgressChangedEventInterval = 100)
```

#### Parameters

`minSupport` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum support threshold for identifying frequent itemsets.

`minConfidence` [double](https://learn.microsoft.com/dotnet/api/system.double)

The minimum confidence threshold for generating association rules.

`itemConversionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemConversionRule.md)\>

An optional collection of <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemConversionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for converting items.

`itemExclusionRules` [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemExclusionRule.md)\>

An optional collection of <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for excluding items.

`maxDegreeOfParallelism` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The maximum degree of parallelism to use during the mining process.

`statePartitionsCount` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The number of state partitions used to store shared state across worker threads.

`miningProgressChangedEventInterval` [int](https://learn.microsoft.com/dotnet/api/system.int32)

The interval in milliseconds at which the <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner.MiningProgressChanged" data-throw-if-not-resolved="false"></xref> event is generated.

#### Exceptions

 [ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

<p><strong>Thrown if:</strong></p><ol><li>
            <code class="paramref">minSupport</code> or <code class="paramref">minConfidence</code> is not between 0 and 1;
        </li><li>
            <code class="paramref">maxDegreeOfParallelism</code> is not positive;
        </li><li>
            <code class="paramref">statePartitionsCount</code> is not positive or greater than <code class="paramref">maxDegreeOfParallelism</code>;
        </li><li>
            <code class="paramref">miningProgressChangedEventInterval</code> is not positive.
        </li></ol>

 [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

<p><strong>Thrown if:</strong></p><ol><li>
            <code class="paramref">itemConversionRules</code> is empty or contains <code>null</code> or duplicates;
        </li><li>
            <code class="paramref">itemExclusionRules</code> is empty or contains <code>null</code> items.
        </li></ol>

## Properties

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_ItemConversionRules"></a> ItemConversionRules

Gets the collection of <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemConversionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for converting items.

```csharp
public IReadOnlyCollection<ItemConversionRule> ItemConversionRules { get; }
```

#### Property Value

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemConversionRule](MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemConversionRule.md)\>

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_ItemExclusionRules"></a> ItemExclusionRules

Gets collection of <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemExclusionRule" data-throw-if-not-resolved="false"></xref> objects that define the rules for excluding items.

```csharp
public IReadOnlyCollection<ItemExclusionRule> ItemExclusionRules { get; }
```

#### Property Value

 [IReadOnlyCollection](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlycollection\-1)<[ItemExclusionRule](MarketBasketAnalysis.AssociationRuleMining.Contracts.ItemExclusionRule.md)\>

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_MaxDegreeOfParallelism"></a> MaxDegreeOfParallelism

Gets the maximum degree of parallelism to use during the mining process.

```csharp
public int MaxDegreeOfParallelism { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_MinConfidence"></a> MinConfidence

Gets the minimum confidence threshold for generating association rules.

```csharp
public double MinConfidence { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_MinSupport"></a> MinSupport

Gets the minimum support threshold for identifying frequent items and frequent item pairs.

```csharp
public double MinSupport { get; }
```

#### Property Value

 [double](https://learn.microsoft.com/dotnet/api/system.double)

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_MiningProgressChangedEventInterval"></a> MiningProgressChangedEventInterval

Gets the interval in milliseconds at which the <xref href="MarketBasketAnalysis.AssociationRuleMining.Contracts.IMiner.MiningProgressChanged" data-throw-if-not-resolved="false"></xref> event is generated.

```csharp
public int MiningProgressChangedEventInterval { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

### <a id="MarketBasketAnalysis_AssociationRuleMining_Contracts_MiningParameters_StatePartitionsCount"></a> StatePartitionsCount

Gets the number of state partitions used to store shared state across worker threads.

```csharp
public int StatePartitionsCount { get; }
```

#### Property Value

 [int](https://learn.microsoft.com/dotnet/api/system.int32)

