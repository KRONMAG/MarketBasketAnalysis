using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    internal sealed class SearchForFrequentItemsResult
    {
        public static readonly SearchForFrequentItemsResult Empty =
            new SearchForFrequentItemsResult(new Dictionary<Item, int>(), 0);

        public IReadOnlyDictionary<Item, int> FrequentItems { get; }

        public int TransactionsCount { get; }

        public SearchForFrequentItemsResult(IReadOnlyDictionary<Item, int> frequentItems, int transactionsCount)
        {
            if (transactionsCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(transactionsCount),
                    transactionsCount,
                    "Transactions count must be greater than or equal to zero.");
            }

            FrequentItems = frequentItems ?? throw new ArgumentNullException(nameof(frequentItems));
            TransactionsCount = transactionsCount;
        }

        public void Deconstruct(out IReadOnlyDictionary<Item, int> frequentItems, out int transactionsCount)
        {
            frequentItems = FrequentItems;
            transactionsCount = TransactionsCount;
        }
    }
}