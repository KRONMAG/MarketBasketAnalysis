using System.Diagnostics.CodeAnalysis;

namespace MarketBasketAnalysis.Server.Data;

[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.UnlimitedStringLength")]
public class AssociationRuleSet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public int TransactionCount { get; set; }

    public bool IsAvailable { get; set; }

    public ICollection<ItemChunk> ItemChunks { get; } = [];

    public ICollection<AssociationRuleChunk> AssociationRuleChunks { get; } = [];
}