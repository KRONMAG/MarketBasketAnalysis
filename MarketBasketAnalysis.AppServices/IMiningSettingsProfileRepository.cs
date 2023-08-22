using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.AppServices
{
    public interface IMiningSettingsProfileRepository
    {
        Task<IReadOnlyCollection<string>> GetProfileNamesAsync();

        Task<MiningSettingsProfile> GetAsync(string profileName);

        Task AddAsync(MiningSettingsProfile profile);

        Task RemoveAsync(string profileName);
    }
}
