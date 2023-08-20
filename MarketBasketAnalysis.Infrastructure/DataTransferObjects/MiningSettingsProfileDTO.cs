using System.Collections.Generic;
#pragma warning disable CA2227 // Collection properties should be read only
#pragma warning disable CA1002 // Do not expose generic lists

namespace MarketBasketAnalysis.Infrastructure.DataTransferObjects;

public class MiningSettingsProfileDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double MinSupport { get; set; }

    public double MinConfidence { get; set; }

    public List<ItemExclusionRuleDTO> ItemExclusionRules { get; set; } = new List<ItemExclusionRuleDTO>();

    public List<ItemConversionRuleDTO> ItemConversionRules { get; set; } = new List<ItemConversionRuleDTO>();
}