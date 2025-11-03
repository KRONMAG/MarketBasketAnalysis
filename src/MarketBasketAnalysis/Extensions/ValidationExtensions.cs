using MarketBasketAnalysis.Mining;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketBasketAnalysis.Extensions
{
    internal static class ValidationExtensions
    {
        public static void Validate(this IReadOnlyCollection<ItemExclusionRule> itemExclusionRules, string paramName)
        {
            if (itemExclusionRules == null)
                throw new ArgumentNullException(paramName);

            if (itemExclusionRules.Count == 0)
            {
                throw new ArgumentException("Collection of item exclusion rules cannot be empty.", paramName);
            }

            if (itemExclusionRules.Any(item => item == null))
            {
                throw new ArgumentException(
                    "Collection of item exclusion rules cannot contain null items.", paramName);
            }
        }

        public static void Validate(this IReadOnlyCollection<ItemConversionRule> itemConversionRules, string paramName)
        {
            if (itemConversionRules == null)
                throw new ArgumentNullException(paramName);

            if (itemConversionRules.Count == 0)
            {
                throw new ArgumentException("Collection of item conversion rules cannot be empty.", paramName);
            }

            if (itemConversionRules.Any(item => item == null))
            {
                throw new ArgumentException(
                    "Collection of item conversion rules cannot contain null items.", paramName);
            }

            if (itemConversionRules.Distinct().Count() != itemConversionRules.Count)
            {
                throw new ArgumentException(
                    "Collection of item conversion rules cannot contain same items.", paramName);
            }
        }
    }
}