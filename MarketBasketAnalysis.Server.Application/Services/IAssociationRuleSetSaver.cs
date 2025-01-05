using MarketBasketAnalysis.Common.Protos;

namespace MarketBasketAnalysis.Server.Application.Services;

public interface IAssociationRuleSetSaver
{
    Task SaveAssociationRuleSetInfoAsync(AssociationRuleSetInfoMessage message, CancellationToken token = default);

    Task SaveItemChunk(ItemChunkMessage message, CancellationToken token = default);

    Task SaveAssociationRuleChunk(AssociationRuleChunkMessage message, CancellationToken token = default);

    Task RollbackChangesAsync(CancellationToken token = default);

    Task MarkSetAsAvailableAsync(CancellationToken token = default);
}