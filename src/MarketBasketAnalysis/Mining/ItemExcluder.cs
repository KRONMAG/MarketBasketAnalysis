// Ignore Spelling: Excluder

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MarketBasketAnalysis.Extensions;

namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    internal sealed class ItemExcluder : IItemExcluder
    {
        #region Fields and Properties
        private readonly IReadOnlyCollection<ItemExclusionRule> _itemExclusionRules;

        private readonly ConcurrentDictionary<Item, int> _allowedItems;
        private readonly ConcurrentDictionary<Item, int> _notAllowedItems;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemExcluder"/> class with the specified collection of exclusion rules.
        /// </summary>
        /// <param name="itemExclusionRules">
        /// A collection of <see cref="ItemExclusionRule"/> objects that define the rules for excluding items.
        /// Each rule specifies the criteria for determining whether an item should be excluded.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="itemExclusionRules"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="itemExclusionRules"/> is empty or contains <c>null</c> items.
        /// </exception>
        /// <remarks>
        /// The enumeration of the <paramref name="itemExclusionRules"/> may be performed multiple times.
        /// </remarks>
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
        /// <inheritdoc />
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
