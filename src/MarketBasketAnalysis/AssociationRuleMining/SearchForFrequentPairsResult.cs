using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class SearchForFrequentPairsResult
    {
        public static readonly SearchForFrequentPairsResult Empty =
            new SearchForFrequentPairsResult(new Dictionary<(Item, Item), int>());

        public IReadOnlyDictionary<(Item, Item), int> FrequentPairs { get; }

        public SearchForFrequentPairsResult(IReadOnlyDictionary<(Item, Item), int> frequentPairs)
        {
            FrequentPairs = frequentPairs ?? throw new ArgumentNullException(nameof(frequentPairs));
        }
    }
}