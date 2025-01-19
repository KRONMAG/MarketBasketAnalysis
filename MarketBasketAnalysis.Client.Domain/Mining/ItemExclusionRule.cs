using System;
using static System.StringComparison;

namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public class ItemExclusionRule
    {
        #region Fields and Properties

        public string Pattern { get; }

        public bool ExactMatch { get; }

        public bool IgnoreCase { get; }

        public bool ApplyToItems { get; }

        public bool ApplyToGroups { get; }

        #endregion

        #region Constructors

        public ItemExclusionRule(string pattern, bool exactMatch, bool ignoreCase,
            bool applyToItems, bool applyToGroups)
        {
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));

            if (!applyToItems && !applyToGroups)
                throw new ArgumentException("Item exclusion rule must be applicable to items or groups.");

            ExactMatch = exactMatch;
            IgnoreCase = ignoreCase;
            ApplyToItems = applyToItems;
            ApplyToGroups = applyToGroups;
        }

        #endregion

        #region Methods

        public bool ShouldExclude(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!item.IsGroup && !ApplyToItems || item.IsGroup && !ApplyToGroups)
                return false;

            var comparisonType = IgnoreCase ? OrdinalIgnoreCase : Ordinal;

            return ExactMatch
                ? item.Name.Equals(Pattern, comparisonType)
                : item.Name.IndexOf(Pattern, comparisonType) != -1;
        }

        #endregion
    }
}