namespace MarketBasketAnalysis.Infrastructure.DataTransferObjects;

public class ItemExclusionRuleDTO
{
    public int Id { get; set; }

    public string ItemPattern { get; set; } = null!;

    public bool ExactMatch { get; set; }

    public bool IgnoreCase { get; set; }

    public int MiningProfileId { get; set; }

    public MiningSettingsProfileDTO? MiningProfile { get; set; }
}
