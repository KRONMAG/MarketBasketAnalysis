namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    /// <summary>
    /// Represents the steps of the association rule mining process.
    /// </summary>
    public enum MiningStep
    {
        /// <summary>
        /// The step where frequent items are identified based on the minimum support threshold.
        /// </summary>
        SearchForFrequentItems,

        /// <summary>
        /// The step where frequent pairs are generated from the frequent items.
        /// </summary>
        SearchForFrequentPairs,

        /// <summary>
        /// The step where association rules are generated from the frequent itemsets.
        /// </summary>
        GenerateAssociationRules,
    }
}