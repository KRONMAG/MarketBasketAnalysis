using System;

namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Provides data for the event that occurs when the mining stage changes.
    /// </summary>
    public sealed class MiningStageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current stage of the mining process.
        /// </summary>
        /// <value>The current <see cref="MiningStage"/> of the mining operation.</value>
        public MiningStage Stage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiningStageChangedEventArgs"/> class.
        /// </summary>
        /// <param name="stage">The current mining stage.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when stage is not a defined value in the <see cref="MiningStage"/>.
        /// </exception>
        internal MiningStageChangedEventArgs(MiningStage stage)
        {
            if (!Enum.IsDefined(typeof(MiningStage), stage))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(stage),
                    "Mining stage must represent actual enum value.");
            }

            Stage = stage;
        }
    }
}