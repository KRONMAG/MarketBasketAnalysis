using MarketBasketAnalysis.Mining;

namespace MarketBasketAnalysis.UnitTests
{
    public class ItemConverterTests
    {
        private readonly Item _item;
        private readonly Item _group;
        private readonly ItemConversionRule _itemConversionRule;
        private readonly ItemConverter _itemConverter;

        public ItemConverterTests()
        {
            _item = new(1, "item", false);
            _group = new(2, "group", true);
            _itemConversionRule = new ItemConversionRule(_item, _group);
            _itemConverter = new([_itemConversionRule]);
        }

        [Fact]
        public void Ctor_ConversionRulesIsNull_ThrowsArgumentNullException() =>
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ItemConverter(null));

        [Fact]
        public void Ctor_ConversionRulesIsEmpty_ThrowsArgumentException() =>
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ItemConverter([]));

        [Fact]
        public void Ctor_ConversionRulesContainNull_ThrowsArgumentException() =>
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ItemConverter([null]));

        [Fact]
        public void Ctor_ConversionRulesContainDuplicates_ThrowsArgumentException() =>
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ItemConverter([_itemConversionRule, _itemConversionRule]));

        [Fact]
        public void TryConvert_ItemIsNull_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => _itemConverter.TryConvert(null, out _));

        [Fact]
        public void TryConvert_ItemIsNotInRules_ReturnsFalse()
        {
            // Arrange
            var item = new Item(3, "item", false);

            // Act
            var result = _itemConverter.TryConvert(item, out var group);

            // Assert
            Assert.False(result);
            Assert.Null(group);
        }

        [Fact]
        public void TryConvert_ItemIsInRules_ReturnsTrue()
        {
            // Arrange
            var item = new Item(_item.Id, _item.Name, _item.IsGroup);

            // Act
            var result = _itemConverter.TryConvert(item, out var group);

            // Assert
            Assert.True(result);
            Assert.NotNull(group);
            Assert.Equal(_group, group);
        }
    }
}
