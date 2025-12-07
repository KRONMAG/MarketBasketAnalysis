using System;
using System.Collections.Generic;
using System.Linq;
using MarketBasketAnalysis.Mining;
#pragma warning disable SA1600 // Elements should be documented

namespace MarketBasketAnalysis.Extensions
{
    internal static class ValidationExtensions
    {
        public static void Validate(this IReadOnlyCollection<ItemExclusionRule> itemExclusionRules, string paramName)
        {
            if (itemExclusionRules == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (itemExclusionRules.Count == 0)
            {
                throw new ArgumentException("Collection of item exclusion rules cannot be empty.", paramName);
            }

            if (itemExclusionRules.Contains(null))
            {
                throw new ArgumentException(
                    "Collection of item exclusion rules cannot contain null items.", paramName);
            }
        }

        public static void Validate(this IReadOnlyCollection<ItemConversionRule> itemConversionRules, string paramName)
        {
            if (itemConversionRules == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (itemConversionRules.Count == 0)
            {
                throw new ArgumentException("Collection of item conversion rules cannot be empty.", paramName);
            }

            if (itemConversionRules.Contains(null))
            {
                throw new ArgumentException(
                    "Collection of item conversion rules cannot contain null items.", paramName);
            }

            if (itemConversionRules.GroupBy(rule => rule.SourceItem).Any(g => g.Count() > 1))
            {
                throw new ArgumentException(
                    "Collection of item conversion rules cannot contain multiple rules for the same source item.",
                    paramName);
            }

            if (itemConversionRules.Distinct().Count() != itemConversionRules.Count)
            {
                throw new ArgumentException(
                    "Collection of item conversion rules cannot contain duplicates.", paramName);
            }
        }
    }
}