using MarketBasketAnalysis.Extensions;

namespace MarketBasketAnalysis.UnitTests;

public class AssociationRuleExtensionsTests
{
    private readonly AssociationRule _abAssociationRule;
    private readonly AssociationRule _baAssociationRule;
    private readonly AssociationRule _bcAssociationRule;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly AssociationRule _cbAssociationRule;
    private readonly AssociationRule _cdAssociationRule;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly AssociationRule _dcAssociationRule;

    private readonly List<AssociationRule> _firstAssociationRuleSet;
    private readonly List<AssociationRule> _secondAssociationRuleSet;

    public AssociationRuleExtensionsTests()
    {
        var itemA = new Item(1, "A", false);
        var itemB = new Item(2, "B", false);
        var itemC = new Item(3, "C", false);
        var itemD = new Item(4, "D", false);

        _abAssociationRule = CreateAssociationRule(itemA, itemB);
        _baAssociationRule = CreateAssociationRule(itemB, itemA);
        _bcAssociationRule = CreateAssociationRule(itemB, itemC);
        _cbAssociationRule = CreateAssociationRule(itemC, itemB);
        _cdAssociationRule = CreateAssociationRule(itemC, itemD);
        _dcAssociationRule = CreateAssociationRule(itemD, itemC);

        _firstAssociationRuleSet =
        [
            _abAssociationRule,
            _baAssociationRule,
            _bcAssociationRule,
            _cdAssociationRule
        ];
        _secondAssociationRuleSet =
        [
            _abAssociationRule,
            _baAssociationRule,
            _cbAssociationRule,
            _dcAssociationRule
        ];

        static AssociationRule CreateAssociationRule(Item lhsItem, Item rhsItem) =>
            new(lhsItem, rhsItem, 1, 1, 1, 10);
    }

    [Fact]
    public void Except_PassInvalidArguments_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => AssociationRuleExtensions.Except(null, []));
        Assert.Throws<ArgumentException>(() => AssociationRuleExtensions.Except([null], []));
        Assert.Throws<ArgumentNullException>(() => AssociationRuleExtensions.Except([], null));
        Assert.Throws<ArgumentException>(() => AssociationRuleExtensions.Except([], [null]));
    }

    [Fact]
    public void Intersect_PassInvalidArguments_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => AssociationRuleExtensions.Intersect(null, []));
        Assert.Throws<ArgumentException>(() => AssociationRuleExtensions.Intersect([null], []));
        Assert.Throws<ArgumentNullException>(() => AssociationRuleExtensions.Intersect([], null));
        Assert.Throws<ArgumentException>(() => AssociationRuleExtensions.Intersect([], [null]));
    }

    [Fact]
    public void Except_NotIgnoreLinkDirection_ReturnsCorrectResult()
    {
        // Act
        var result = _firstAssociationRuleSet.Except(_secondAssociationRuleSet).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(_bcAssociationRule, result);
        Assert.Contains(_cdAssociationRule, result);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    public void Intersect_SetIsEmpty_ResultIsEmpty(bool isFirstSetEmpty, bool isSecondSetEmpty, bool ignoreLinkDirection)
    {
        // Arrange
        var firstAssociationRuleSet = isFirstSetEmpty ? [] : _firstAssociationRuleSet;
        var secondAssociationRuleSet = isSecondSetEmpty ? [] : _secondAssociationRuleSet;

        // Act
        var result = firstAssociationRuleSet.Intersect(secondAssociationRuleSet, ignoreLinkDirection).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Except_IgnoreLinkDirection_ReturnsCorrectResult()
    {
        // Act
        var result = _firstAssociationRuleSet.Except(_secondAssociationRuleSet, true).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Intersect_NotIgnoreLinkDirection_ReturnsCorrectResult()
    {
        // Act
        var result = _firstAssociationRuleSet.Intersect(_secondAssociationRuleSet).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(_abAssociationRule, result);
        Assert.Contains(_baAssociationRule, result);
    }

    [Fact]
    public void Intersect_IgnoreLinkDirection_ReturnsCorrectResult()
    {
        // Act
        var result = _firstAssociationRuleSet.Intersect(_secondAssociationRuleSet, true).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Contains(_abAssociationRule, result);
        Assert.Contains(_baAssociationRule, result);
        Assert.Contains(_bcAssociationRule, result);
        Assert.Contains(_cdAssociationRule, result);
    }
}
