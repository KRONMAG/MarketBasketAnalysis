using System;

namespace MarketBasketAnalysis.Client.Domain
{
    public sealed class AssociationRulePart : IEquatable<AssociationRulePart>
    {
        public int Id => Item.Id;
        
        public Item Item { get; }

        public double Count { get; }

        public double Support { get; }

        public AssociationRulePart(Item item, int itemCount, int transactionCount)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));

            if (itemCount < 1)
                throw new ArgumentOutOfRangeException(nameof(itemCount), itemCount, "Item count must be positive.");

            if (transactionCount < itemCount)
            {
                throw new ArgumentOutOfRangeException(nameof(transactionCount), transactionCount,
                    "Transaction count must be greater than or equal to item count.");
            }

            Count = itemCount;
            Support = (double)itemCount / transactionCount;
        }

        public bool Equals(AssociationRulePart other)
        {
            if (ReferenceEquals(null, other))
                return false;
            
            if (ReferenceEquals(this, other))
                return true;
            
            return Item.Equals(other.Item);
        }

        public override bool Equals(object obj) =>
            Equals(obj as AssociationRulePart);

        public override int GetHashCode() =>
            Item.GetHashCode();

        public override string ToString() =>
            Item.ToString();
    }
}