using System;

namespace MarketBasketAnalysis.Client.Domain.Mining
{
    public sealed class ItemConversionRule : IEquatable<ItemConversionRule>
    {
        #region Fields and Properties

        public Item SourceItem { get; }

        public Item TargetItem { get; }

        #endregion

        #region Constructors

        public ItemConversionRule(Item sourceItem, Item targetItem)
        {
            if (sourceItem == null)
                throw new ArgumentNullException(nameof(sourceItem));

            if (sourceItem.IsGroup)
            {
                throw new ArgumentException(
                    "Source item cannot be a group because it represents transaction data item.",
                    nameof(sourceItem));
            }

            if (targetItem == null)
                throw new ArgumentNullException(nameof(targetItem));

            if (!targetItem.IsGroup)
                throw new ArgumentException("Target item must be group.", nameof(targetItem));

            SourceItem = sourceItem;
            TargetItem = targetItem;
        }

        #endregion

        #region Methods

        public bool Equals(ItemConversionRule other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return SourceItem.Equals(other.SourceItem) && TargetItem.Equals(other.TargetItem);
        }

        public override bool Equals(object obj) =>
            Equals(obj as ItemConversionRule);

        public override int GetHashCode() =>
            SourceItem.GetHashCode() * 397 ^ TargetItem.GetHashCode();

        public override string ToString() =>
            $"{SourceItem.Name} -> {TargetItem.Name}";

        #endregion 
    }
}
