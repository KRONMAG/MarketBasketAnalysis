namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    public sealed class MinerFactory : IMinerFactory
    {
        /// <inheritdoc />
        public IMiner Create() =>
            new Miner(
                itemConversionRules => new ItemConverter(itemConversionRules),
                itemExclusionRules => new ItemExcluder(itemExclusionRules));
    }
}