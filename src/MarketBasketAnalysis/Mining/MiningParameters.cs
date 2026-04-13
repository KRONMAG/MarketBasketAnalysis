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
        public double MinSupport { get; }

        /// <summary>
        /// Gets the minimum confidence threshold for generating association rules.
        /// </summary>
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
        public int DegreeOfParallelism { get; }

        /// <summary>
        /// Gets the number of state partitions used to store shared state across worker threads.
        /// </summary>
        public int StatePartitionCount { get; }

        /// <summary>
        /// Gets the interval in milliseconds at which the <see cref="IMiner.MiningProgressUpdated"/> event is generated.
        /// </summary>
        public int MiningProgressInterval { get; }
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
        /// <param name="statePartitionCount">The number of state partitions used to store shared state across worker threads.</param>
        /// <param name="miningProgressInterval">The interval in milliseconds at which the <see cref="IMiner.MiningProgressUpdated"/> event is generated.</param>
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
        ///             <paramref name="degreeOfParallelism"/> is not positive;
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="statePartitionCount"/> is not positive or greater than <paramref name="degreeOfParallelism"/>;
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="miningProgressInterval"/> is not positive.
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
            int degreeOfParallelism = 1,
            int statePartitionCount = 1,
            int miningProgressInterval = 100)
        {
            ValidateParameters(
                minSupport,
                minConfidence,
                itemConversionRules,
                itemExclusionRules,
                degreeOfParallelism,
                statePartitionCount,
                miningProgressInterval);

            MinSupport = minSupport;
            MinConfidence = minConfidence;
            ItemConversionRules = itemConversionRules;
            ItemExclusionRules = itemExclusionRules;
            DegreeOfParallelism = degreeOfParallelism;
            StatePartitionCount = statePartitionCount;
            MiningProgressInterval = miningProgressInterval;
        }

        private static void ValidateParameters(
            double minSupport,
            double minConfidence,
            IReadOnlyCollection<ItemConversionRule> itemConversionRules,
            IReadOnlyCollection<ItemExclusionRule> itemExclusionRules,
            int degreeOfParallelism,
            int statePartitionCount,
            int miningProgressInterval)
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

            if (degreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(degreeOfParallelism),
                    degreeOfParallelism,
                    "Degree of parallelism must be positive.");
            }

            if (statePartitionCount < 1 || statePartitionCount > degreeOfParallelism)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statePartitionCount),
                    statePartitionCount,
                    "State partition count must be positive and less than or equal to degree of parallelism.");
            }

            if (miningProgressInterval < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(miningProgressInterval),
                    miningProgressInterval,
                    "Mining progress interval must be positive.");
            }

            itemConversionRules?.Validate(nameof(itemConversionRules));
            itemExclusionRules?.Validate(nameof(itemExclusionRules));
        }
        #endregion
    }
}