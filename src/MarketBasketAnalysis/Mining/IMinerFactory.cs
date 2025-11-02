namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Defines an interface for factory interface for creating <see cref="IMiner"/> instances.
    /// </summary>
    /// <remarks>
    /// Use this interface to obtain new miners for performing association rule mining on transaction data.
    /// </remarks>
    public interface IMinerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IMiner"/> instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="IMiner"/> that can be used to perform association rule mining.
        /// </returns>
        IMiner Create();
    }
}