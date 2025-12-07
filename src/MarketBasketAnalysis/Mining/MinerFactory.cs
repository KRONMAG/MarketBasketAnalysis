namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    internal sealed class MinerFactory : IMinerFactory
    {
        /// <inheritdoc />
        public IMiner Create() =>
            new Miner(
                itemConversionRules => new ItemConverter(itemConversionRules),
                itemExclusionRules => new ItemExcluder(itemExclusionRules));
    }
}