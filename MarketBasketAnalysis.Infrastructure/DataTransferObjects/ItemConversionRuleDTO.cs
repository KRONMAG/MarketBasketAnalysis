namespace MarketBasketAnalysis.Infrastructure.DataTransferObjects
{
    public class ItemConversionRuleDTO
    {
        public int Id { get; set; }

        public string Item { get; set; } = null!;

        public string Group { get; set; } = null!;

        public int MiningProfileId { get; set; }

        public MiningSettingsProfileDTO? MiningProfile { get; set; }
    }
}
