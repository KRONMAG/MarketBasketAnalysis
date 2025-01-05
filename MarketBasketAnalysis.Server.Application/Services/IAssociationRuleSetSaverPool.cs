namespace MarketBasketAnalysis.Server.Application.Services;

public interface IAssociationRuleSetSaverPool : IDisposable
{
    Task<IAssociationRuleSetSaver?> TryRentAsync(string key);

#pragma warning disable CA1716 // Identifiers should not match keywords
    Task ReturnAsync(string key);
#pragma warning restore CA1716

    Task RemoveAsync(string key);
}
