using MarketBasketAnalysis.Common.Protos;

namespace MarketBasketAnalysis.Server.Application.Services;

public interface IAssociationRuleSetLoader
{
    Task<AssociationRuleSetInfoMessage> LoadAssociationRuleSetInfoAsync(string associationRuleSetName,
        CancellationToken token = default);

    IAsyncEnumerable<ItemChunkMessage> LoadItemChunksAsync(string associationRuleSetName,
        CancellationToken token = default);

    IAsyncEnumerable<AssociationRuleChunkMessage> LoadAssociationRuleChunksAsync(string associationRuleSetName,
        CancellationToken token = default);
}