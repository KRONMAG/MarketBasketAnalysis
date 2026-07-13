using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    internal sealed class SearchForItemsetsResult
    {
        public static readonly SearchForItemsetsResult Empty =
            new SearchForItemsetsResult(new Dictionary<(Item, Item), int>());

        public IReadOnlyDictionary<(Item, Item), int> Itemsets { get; }

        public SearchForItemsetsResult(IReadOnlyDictionary<(Item, Item), int> itemsets)
        {
            Itemsets = itemsets ?? throw new ArgumentNullException(nameof(itemsets));
        }
    }
}