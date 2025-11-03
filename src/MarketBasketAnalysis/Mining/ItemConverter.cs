using System;
using System.Collections.Generic;
using System.Linq;
using MarketBasketAnalysis.Extensions;

namespace MarketBasketAnalysis.Mining
{
    /// <inheritdoc />
    public sealed class ItemConverter : IItemConverter
    {
        #region Fields and Properties

        private readonly Dictionary<Item, ItemConversionRule> _itemConversionRules;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemConverter"/> class with the specified collection of conversion rules.
        /// </summary>
        /// <param name="itemConversionRules">
        /// A collection of <see cref="ItemConversionRule"/> objects that define the rules for converting items.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="itemConversionRules"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="itemConversionRules"/> is empty or contains <c>null</c> or same rules.
        /// </exception>
        public ItemConverter(IReadOnlyCollection<ItemConversionRule> itemConversionRules)
        {
            if (itemConversionRules == null)
                throw new ArgumentNullException(nameof(itemConversionRules));

            itemConversionRules.Validate(nameof(itemConversionRules));

            _itemConversionRules = itemConversionRules.ToDictionary(rule => rule.SourceItem);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool TryConvert(Item item, out Item group)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_itemConversionRules.TryGetValue(item, out var replacementRule))
            {
                group = replacementRule.TargetItem;

                return true;
            }

            group = default;

            return false;
        }

        #endregion
    }
}