using System;

namespace MarketBasketAnalysis.Mining
{
    /// <summary>
    /// Defines a method for determining whether an item should be replaced with its corresponding group.
    /// </summary>
    public interface IItemConverter
    {
        /// <summary>
        /// Determines whether the specified item should be replaced with a group.
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to evaluate.</param>
        /// <param name="group">
        /// When this method returns, contains the group of the specified <paramref name="item"/>,
        /// if a replacement is required; otherwise, contains <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the item should be replaced with a group representation; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <c>null</c>.</exception>
        bool TryConvert(Item item, out Item group);
    }
}
