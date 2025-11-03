# <a id="MarketBasketAnalysis_Mining"></a> Namespace MarketBasketAnalysis.Mining

### Classes

 [ItemConversionRule](MarketBasketAnalysis.Mining.ItemConversionRule.md)

Represents a rule for converting one item into another.

 [ItemConverter](MarketBasketAnalysis.Mining.ItemConverter.md)

 [ItemExcluder](MarketBasketAnalysis.Mining.ItemExcluder.md)

 [ItemExclusionRule](MarketBasketAnalysis.Mining.ItemExclusionRule.md)

Represents a rule for excluding items or groups from association rule mining.

 [Miner](MarketBasketAnalysis.Mining.Miner.md)

 [MinerFactory](MarketBasketAnalysis.Mining.MinerFactory.md)

 [MiningParameters](MarketBasketAnalysis.Mining.MiningParameters.md)

Represents the parameters used for mining association rules.

### Interfaces

 [IItemConverter](MarketBasketAnalysis.Mining.IItemConverter.md)

Defines a method for determining whether an item should be replaced with its corresponding group representation.

 [IItemExcluder](MarketBasketAnalysis.Mining.IItemExcluder.md)

Defines a method to determine whether a specific item should be excluded during association rule mining.

 [IMiner](MarketBasketAnalysis.Mining.IMiner.md)

Defines an interface for performing association rule mining based on transaction data.

 [IMinerFactory](MarketBasketAnalysis.Mining.IMinerFactory.md)

Defines an interface for factory interface for creating <xref href="MarketBasketAnalysis.Mining.IMiner" data-throw-if-not-resolved="false"></xref> instances.

### Enums

 [MiningStage](MarketBasketAnalysis.Mining.MiningStage.md)

Represents the stages of the association rule mining process.

