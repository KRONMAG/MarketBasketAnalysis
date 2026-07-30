// Ignore Spelling: Excluder

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.AssociationRuleMining.Extensions;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class ItemExcluder : IItemExcluder
    {
        #region Fields and Properties
        private readonly IReadOnlyCollection<ItemExclusionRule> _itemExclusionRules;

        private readonly ConcurrentDictionary<Item, int> _allowedItems;
        private readonly ConcurrentDictionary<Item, int> _notAllowedItems;
        #endregion

        #region Constructors
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Possibility of multiple enumeration is specified in docs.")]
        public ItemExcluder(IReadOnlyCollection<ItemExclusionRule> itemExclusionRules)
        {
            itemExclusionRules.Validate(nameof(itemExclusionRules));

            _itemExclusionRules = itemExclusionRules;

            _allowedItems = new ConcurrentDictionary<Item, int>();
            _notAllowedItems = new ConcurrentDictionary<Item, int>();
        }
        #endregion

        #region Methods
        public bool ShouldExclude(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_allowedItems.ContainsKey(item))
            {
                return false;
            }

            if (_notAllowedItems.ContainsKey(item))
            {
                return true;
            }

            var shouldExclude = _itemExclusionRules.Any(er => er.ShouldExclude(item));

            if (shouldExclude)
            {
                _notAllowedItems.TryAdd(item, default);
            }
            else
            {
                _allowedItems.TryAdd(item, default);
            }

            return shouldExclude;
        }
        #endregion
    }
}
