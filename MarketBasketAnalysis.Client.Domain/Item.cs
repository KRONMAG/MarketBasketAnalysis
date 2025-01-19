using System;

namespace MarketBasketAnalysis.Client.Domain
{
    public sealed class Item : IEquatable<Item>
    {
        public int Id { get; }

        public string Name { get; }

        public bool IsGroup { get; }

        public Item(int id, string name, bool isGroup)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Id = id;
            IsGroup = isGroup;
        }

        public bool Equals(Item other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        public override bool Equals(object obj) =>
            Equals(obj as Item);

        public override int GetHashCode() =>
            Id.GetHashCode();

        public override string ToString() => Name;
    }
}