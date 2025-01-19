using System;

namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public class MiningParameters
    {
        public double MinSupport { get; }

        public double MinConfidence { get; }

        public IItemConverter ItemConverter { get; }

        public IItemExcluder ItemExcluder { get; }

        public int DegreeOfParallelism { get; }

        public MiningParameters(double minSupport, double minConfidence, IItemConverter itemConverter = null,
            IItemExcluder itemExcluder = null, int degreeOfParallelism = 8)
        {
            if (minSupport < 0 || minSupport > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minSupport), minSupport,
                    "Minimum support threshold must be between 0 and 1.");
            }

            if (minConfidence < 0 || minConfidence > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minConfidence), minConfidence,
                    "Minimum confidence threshold must be between 0 and 1.");
            }

            if (degreeOfParallelism < 1 || degreeOfParallelism > 512)
            {
                throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism), degreeOfParallelism,
                    "Degree of parallelism must be between 1 and 512.");
            }

            MinSupport = minSupport;
            MinConfidence = minConfidence;
            ItemConverter = itemConverter;
            ItemExcluder = itemExcluder;
            DegreeOfParallelism = degreeOfParallelism;
        }
    }
}