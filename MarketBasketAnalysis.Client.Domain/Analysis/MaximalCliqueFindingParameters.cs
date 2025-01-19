using System;

namespace MarketBasketAnalysis.Client.Domain.Analysis
{
    public class MaximalCliqueFindingParameters
    {
        public int MinCliqueSize { get; }

        public int MaxCliqueSize { get; }

        public bool IgnoreOneWayLinks { get; }

        public MaximalCliqueFindingParameters(int minCliqueSize, int maxCliqueSize, bool ignoreOneWayLinks = false)
        {
            if (minCliqueSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minCliqueSize), minCliqueSize,
                    "Minimum clique size must be greater than zero.");
            }

            if (maxCliqueSize < minCliqueSize)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCliqueSize), maxCliqueSize,
                    "Maximum clique size must be greater than or equal to minimum clique size.");
            }

            MinCliqueSize = minCliqueSize;
            MaxCliqueSize = maxCliqueSize;
            IgnoreOneWayLinks = ignoreOneWayLinks;
        }
    }
}