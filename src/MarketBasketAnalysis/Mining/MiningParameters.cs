using System;
using System.Collections.Generic;
using MarketBasketAnalysis.Extensions;

namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Represents the parameters used for mining association rules.
    /// </summary>
    public sealed class MiningParameters
    {
        #region Fields and Properties
        /// <summary>
        /// Gets the minimum support threshold for identifying frequent itemsets.
        /// </summary>
        /// <remarks>
        /// The value must be between 0 and 1, where 0 means no support is required, and 1 means the itemset must appear in all transactions.
        /// </remarks>
        public double MinSupport { get; }

        /// <summary>
        /// Gets the minimum confidence threshold for generating association rules.
        /// </summary>
        /// <remarks>
        /// The value must be between 0 and 1, where 0 means no confidence is required, and 1 means the rule must always hold true.
        /// </remarks>
        public double MinConfidence { get; }

        /// <summary>
        /// Gets the collection of <see cref="ItemConversionRule"/> objects that define the rules for converting items.
        /// </summary>
        public IReadOnlyCollection<ItemConversionRule> ItemConversionRules { get; }

        /// <summary>
        /// Gets collection of <see cref="ItemExclusionRule"/> objects that define the rules for excluding items.
        /// </summary>>
        public IReadOnlyCollection<ItemExclusionRule> ItemExclusionRules { get; }

        /// <summary>
        /// Gets the degree of parallelism to use during the mining process.
        /// </summary>
        /// <remarks>
        /// The value must be between 1 and 512, where higher values allow for more parallel processing.
        /// </remarks>
        public int DegreeOfParallelism { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MiningParameters"/> class.
        /// </summary>
        /// <param name="minSupport">The minimum support threshold for identifying frequent itemsets.</param>
        /// <param name="minConfidence">The minimum confidence threshold for generating association rules.</param>
        /// <param name="itemConversionRules">An optional collection of <see cref="ItemConversionRule"/> objects that define the rules for converting items.</param>
        /// <param name="itemExclusionRules">An optional collection of <see cref="ItemExclusionRule"/> objects that define the rules for excluding items.</param>
        /// <param name="degreeOfParallelism">The degree of parallelism to use during the mining process.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="number">
        ///     <listheader>
        ///         <description>Thrown if:</description>
        ///     </listheader>
        ///     <item>
        ///         <description>
        ///             <paramref name="minSupport"/> or <paramref name="minConfidence"/> is not between 0 and 1;
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="degreeOfParallelism"/> is not between 1 and 512.
        ///         </description>
        ///     </item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="number">
        ///     <listheader>
        ///         <description>Thrown if:</description>
        ///     </listheader>
        ///     <item>
        ///         <description>
        ///             <paramref name="itemConversionRules"/> is empty or contains <c>null</c> or duplicates;
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="itemExclusionRules"/> is empty or contains <c>null</c> items.
        ///         </description>
        ///     </item>
        /// </list>
        /// </exception>
        public MiningParameters(
            double minSupport,
            double minConfidence,
            IReadOnlyCollection<ItemConversionRule> itemConversionRules = null,
            IReadOnlyCollection<ItemExclusionRule> itemExclusionRules = null,
            int degreeOfParallelism = 1)
        {
            if (minSupport < 0 || minSupport > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minSupport),
                    minSupport,
                    "Minimum support threshold must be between 0 and 1.");
            }

            if (minConfidence < 0 || minConfidence > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minConfidence),
                    minConfidence,
                    "Minimum confidence threshold must be between 0 and 1.");
            }

            if (degreeOfParallelism < 1 || degreeOfParallelism > 512)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(degreeOfParallelism),
                    degreeOfParallelism,
                    "Degree of parallelism must be between 1 and 512.");
            }

            if (itemConversionRules != null)
            {
                itemConversionRules.Validate(nameof(itemConversionRules));
            }

            if (itemExclusionRules != null)
            {
                itemExclusionRules.Validate(nameof(itemExclusionRules));
            }

            MinSupport = minSupport;
            MinConfidence = minConfidence;
            ItemConversionRules = itemConversionRules;
            ItemExclusionRules = itemExclusionRules;
            DegreeOfParallelism = degreeOfParallelism;
        }
        #endregion
    }
}