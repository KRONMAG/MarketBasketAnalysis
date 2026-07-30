using System;

namespace MarketBasketAnalysis.Models
{
    /// <summary>
    /// Represents a part of an association rule, either the left-hand side or the right-hand side.
    /// </summary>
    public sealed class AssociationRulePart : IEquatable<AssociationRulePart>
    {
        #region Method and Properties
        /// <summary>
        /// Gets the unique identifier of the item associated with this part of the rule.
        /// </summary>
        public int Id => Item.Id;

        /// <summary>
        /// Gets the item associated with this part of the rule.
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Gets the number of transactions that contain the item in this part of the rule.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets the support of the item in this part of the rule,
        /// which is the proportion of transactions that contain the item.
        /// </summary>
        public double Support { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AssociationRulePart"/> class.
        /// </summary>
        /// <param name="item">The item associated with this part of the rule.</param>
        /// <param name="itemCount">The number of transactions that contain the item.</param>
        /// <param name="transactionsCount">The total number of transactions.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="itemCount"/> is less than 1 or
        /// if <paramref name="transactionsCount"/> is less than <paramref name="itemCount"/>.
        /// </exception>
        internal AssociationRulePart(Item item, int itemCount, int transactionsCount)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));

            if (itemCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(itemCount), itemCount, "Item count must be positive.");
            }

            if (itemCount > transactionsCount)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(itemCount),
                    itemCount,
                    "Item count must be less than or equal to transactions count.");
            }

            Count = itemCount;
            Support = (double)itemCount / transactionsCount;
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public bool Equals(AssociationRulePart other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Item.Equals(other.Item);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            Equals(obj as AssociationRulePart);

        /// <inheritdoc />
        public override int GetHashCode() =>
            Item.GetHashCode();

        /// <inheritdoc />
        public override string ToString() =>
            Item.ToString();
        #endregion
    }
}