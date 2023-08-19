using System;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Linq;

namespace MarketBasketAnalysis.DomainModel.Mining;

public sealed class Transaction
{
    #region Fields and Properties

    public IReadOnlyList<string> Items { get; }

    public IEnumerable<Itemset> Itemsets
    {
        get
        {
            for (var i = 0; i < Items.Count; i++)
                for (var j = i + 1; j < Items.Count; j++)
                    yield return new Itemset(Items[i], Items[j]);
        }
    }

    #endregion Fields and Properties

    #region Constructors

    public Transaction(IReadOnlyList<string> items)
    {
        Contract.RequiresNotNull(items);
        Contract.Requires(items.Count > 0);
        Contract.RequiresForAll(items, item => !string.IsNullOrWhiteSpace(item));
        Contract.Requires(items.Count == items.Distinct(StringComparer.Ordinal).Count());
        
        Items = items;
    }

    #endregion Constructors
}