using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public interface IMiner
    {
        event EventHandler<double> MiningProgressChanged;

        event EventHandler<MiningStage> MiningStageChanged;

        IReadOnlyCollection<AssociationRule> Mine(IEnumerable<Item[]> transactions,
            MiningParameters parameters);

        Task<IReadOnlyCollection<AssociationRule>> MineAsync(IEnumerable<Item[]> transactions,
            MiningParameters parameters, CancellationToken token = default);
    }
}
