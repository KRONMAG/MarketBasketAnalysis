using System;
using System.Collections.Generic;
using System.Linq;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.AssociationRuleMining.Extensions;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal sealed class ItemConverter : IItemConverter
    {
        #region Fields and Properties
        private readonly Dictionary<Item, ItemConversionRule> _itemConversionRules;
        #endregion

        #region Constructors
        public ItemConverter(IReadOnlyCollection<ItemConversionRule> itemConversionRules)
        {
            itemConversionRules.Validate(nameof(itemConversionRules));

            _itemConversionRules = itemConversionRules.ToDictionary(rule => rule.SourceItem);
        }
        #endregion

        #region Methods
        public bool TryConvert(Item item, out Item group)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_itemConversionRules.TryGetValue(item, out var replacementRule))
            {
                group = replacementRule.TargetItem;

                return true;
            }

            group = null;

            return false;
        }
        #endregion
    }
}