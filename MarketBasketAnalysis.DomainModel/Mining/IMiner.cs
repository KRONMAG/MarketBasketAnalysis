using MarketBasketAnalysis.DomainModel.AssociationRules;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MarketBasketAnalysis.DomainModel.Mining;

public interface IMiner
{
#pragma warning disable CA1003 // Use generic event handler instances
    event EventHandler<double>? MiningProgressChanged;

    event EventHandler<MiningStage>? MiningStageChanged;
#pragma warning restore CA1003 // Use generic event handler instances

    IReadOnlySet<AssociationRule> Mine(IEnumerable<Transaction> transactions, double minSupport,
        double minConfidence, IItemsetConverter? itemsetConverter = null, IItemExcluder? itemExcluder = null,
        CancellationToken token = default);
}
