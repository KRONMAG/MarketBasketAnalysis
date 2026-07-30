using System;

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    /// <summary>
    /// Provides data for the event triggered when one of mining steps starts.
    /// </summary>
    public sealed class MiningStepStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the mining step that has started.
        /// </summary>
        public MiningStep Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiningStepStartedEventArgs"/> class.
        /// </summary>
        /// <param name="step">The mining step that started.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="step"/> does not represent an actual value of <see cref="MiningStep"/>.
        /// </exception>
        internal MiningStepStartedEventArgs(MiningStep step)
        {
            if (!Enum.IsDefined(typeof(MiningStep), step))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(step),
                    "Mining step must represent actual enum value.");
            }

            Step = step;
        }
    }
}