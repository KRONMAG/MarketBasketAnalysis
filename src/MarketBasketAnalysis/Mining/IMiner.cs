using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Defines an interface for performing association rule mining based on transaction data.
    /// </summary>
    [PublicAPI]
    public interface IMiner
    {
        /// <summary>
        /// Event triggered when the mining progress changes.
        /// </summary>
        event EventHandler<MiningProgressChangedEventArgs> MiningProgressChanged;

        /// <summary>
        /// Event triggered when the mining stage changes.
        /// </summary>
        event EventHandler<MiningStageChangedEventArgs> MiningStageChanged;

        /// <summary>
        /// Performs association rule mining.
        /// </summary>
        /// <param name="transactions">A collection of transactions, where each transaction is represented as an collection of items.</param>
        /// <param name="parameters">The mining parameters, including minimum support and confidence thresholds.</param>
        /// <param name="token">A cancellation token to cancel the operation if needed.</param>
        /// <returns>A collection of association rules that meet the specified parameters.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="transactions"/> or <paramref name="parameters"/> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled via the <paramref name="token"/>.
        /// </exception>
        /// <remarks>
        /// The enumeration of the <paramref name="transactions"/> may be performed multiple times.
        /// </remarks>
        IReadOnlyCollection<AssociationRule> Mine(
            IEnumerable<IReadOnlyList<Item>> transactions,
            MiningParameters parameters,
            CancellationToken token = default);
    }
}
