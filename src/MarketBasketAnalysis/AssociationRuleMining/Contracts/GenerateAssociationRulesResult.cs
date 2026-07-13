using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    internal sealed class GenerateAssociationRulesResult
    {
        public static readonly GenerateAssociationRulesResult Empty =
            new GenerateAssociationRulesResult(Array.Empty<AssociationRule>());

        public IReadOnlyCollection<AssociationRule> AssociationRules { get; }

        public GenerateAssociationRulesResult(IReadOnlyCollection<AssociationRule> associationRules)
        {
            AssociationRules = associationRules ?? throw new ArgumentNullException(nameof(associationRules));
        }
    }
}