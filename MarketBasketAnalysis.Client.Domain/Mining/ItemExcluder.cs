using ConcurrentCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public class ItemExcluder : IItemExcluder
    {
        #region Fields and Properties

        private readonly IReadOnlyCollection<ItemExclusionRule> _exclusionRules;

        private readonly ConcurrentHashSet<Item> _allowedItems;
        private readonly ConcurrentHashSet<Item> _notAllowedItems;

        #endregion

        #region Constructors

        public ItemExcluder(IReadOnlyCollection<ItemExclusionRule> exclusionRules)
        {
            if (exclusionRules == null)
                throw new ArgumentNullException(nameof(exclusionRules));

            if (exclusionRules.Any(item => item == null))
            {
                throw new ArgumentException("Collection of item exclusion rules cannot contain null items.",
                    nameof(exclusionRules));
            }

            _exclusionRules = exclusionRules;

            _allowedItems = new ConcurrentHashSet<Item>();
            _notAllowedItems = new ConcurrentHashSet<Item>();
        }

        #endregion

        #region Methods

        public bool ShouldExclude(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_allowedItems.Contains(item))
                return false;

            if (_notAllowedItems.Contains(item))
                return true;

            var shouldExclude = false;

            foreach (var exclusionRule in _exclusionRules)
            {
                if (exclusionRule.ShouldExclude(item))
                {
                    shouldExclude = true;

                    break;
                }
            }

            if (shouldExclude)
                _notAllowedItems.Add(item);
            else
                _allowedItems.Add(item);

            return shouldExclude;
        }

        #endregion
    }
}
