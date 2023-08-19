using MarketBasketAnalysis.DomainModel.AssociationRules.Mining;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Linq;

namespace MarketBasketAnalysis.DomainModel.Mining;

public class ItemExcluder : IItemExcluder
{
    #region Fields and Properties

    private IReadOnlySet<ItemExclusionRule> _exclusionRules;

    #endregion Fields and Properties

    #region Constructors

    public ItemExcluder(IReadOnlySet<ItemExclusionRule> exclusionRules)
    {
        Contract.RequiresNotNull(exclusionRules);
        Contract.RequiresForAll(exclusionRules, item => item != null);

        _exclusionRules = exclusionRules;
    }

    #endregion Constructors

    #region Methods

    public bool ShouldExclude(string item)
    {
        Contract.RequiresNotNullOrWhiteSpace(item);

        return _exclusionRules.Any(rule => rule.ShouldExclude(item));
    }

    #endregion Methods
}