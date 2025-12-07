using System;
using JetBrains.Annotations;

namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Provides data for the event that occurs when mining progress changes.
    /// </summary>
    [PublicAPI]
    public sealed class MiningProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current progress of the mining operation as a value between 0 and 100.
        /// </summary>
        public double Progress { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiningProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="progress">The mining progress value between 0 and 100.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when progress is negative or greater than 100.</exception>
        internal MiningProgressChangedEventArgs(double progress)
        {
            if (progress < 0 || progress > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(progress),
                    "Progress value must be between 0 and 100");
            }

            Progress = progress;
        }
    }
}