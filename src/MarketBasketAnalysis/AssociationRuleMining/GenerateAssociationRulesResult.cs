using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class GenerateAssociationRulesResult
    {
        public IReadOnlyCollection<AssociationRule> AssociationRules { get; }

        public GenerateAssociationRulesResult(IReadOnlyCollection<AssociationRule> associationRules)
        {
            AssociationRules = associationRules ?? throw new ArgumentNullException(nameof(associationRules));
        }
    }
}