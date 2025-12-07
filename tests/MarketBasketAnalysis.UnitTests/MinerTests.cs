// Ignore Spelling: Excluder

using MarketBasketAnalysis.Mining;
using Moq;

namespace MarketBasketAnalysis.UnitTests;

public class MinerTests
{
    private readonly Item _itemA;
    private readonly Item _itemB;
    private readonly Item _itemC;
    private readonly Item _group;
    private readonly IEnumerable<Item[]> _transactions;
    private readonly Mock<IItemConverter> _itemConverterMock;
    private readonly Mock<IItemExcluder> _itemExcluderMock;
    private readonly List<ItemExclusionRule> _itemExclusionRules;
    private readonly List<ItemConversionRule> _itemConversionRules;
    private readonly Miner _miner;

    public MinerTests()
    {
        _itemA = new(1, "A", false);
        _itemB = new(2, "B", false);
        _itemC = new(3, "C", false);
        _group = new(4, "Group", true);

        _transactions =
        [
            [_itemA],
            [_itemA, _itemB, _itemA],
            [_itemA, _itemB],
            [_itemA, _itemC],
            [_itemB, _itemC],
            [_itemA, _itemB, _itemC, _itemB],
        ];

        _itemConverterMock = new();
        _itemExcluderMock = new();

        _itemExclusionRules = [new("A", true, false, true, false)];
        _itemConversionRules = [new(_itemA, _group), new(_itemB, _group)];

        _miner = new Miner(_ => _itemConverterMock.Object, _ => _itemExcluderMock.Object);
    }

    [Fact]
    public void Mine_InvalidArguments_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _miner.Mine(null, new(0, 0)));
        Assert.Throws<ArgumentNullException>(() => _miner.Mine([], null));
        Assert.Throws<AggregateException>(() => _miner.Mine([null], new(0, 0)));
    }

    [Fact]
    public void Mine_WithEmptyTransactions_ReturnsNoAssociationRules()
    {
        // Act
        var actual = _miner.Mine([], new(0, 0));

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void Mine_WithOneItemTransactions_ReturnsNoAssociationRules()
    {
        // Act
        var actual = _miner.Mine([[_itemA]], new(0, 0));

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void Mine_StageChanged_RaisesEvents()
    {
        // Arrange
        var stages = new List<MiningStage>();

        _miner.MiningStageChanged += (_, e) => stages.Add(e.Stage);

        // Act
        _miner.Mine(_transactions, new(0, 0));

        // Assert
        Assert.Equal(
            [MiningStage.FrequentItemSearch, MiningStage.ItemsetSearch, MiningStage.AssociationRuleGeneration],
            stages);
    }

    [Fact]
    public void Mine_ProgressChanged_RaisesEvents()
    {
        // Arrange
        var progressValues = new List<double>();

        _miner.MiningProgressChanged += (_, e) => progressValues.Add(e.Progress);

        // Act
        _miner.Mine(GenerateTransactions(), new(0, 0));

        // Assert
        Assert.NotEmpty(progressValues);
        Assert.All(progressValues, v => Assert.InRange(v, 0, 100));

        IEnumerable<Item[]> GenerateTransactions()
        {
            foreach (var transaction in _transactions)
            {
#pragma warning disable S2925 // "Thread.Sleep" should not be used in tests
                Thread.Sleep(100);
#pragma warning restore S2925 // "Thread.Sleep" should not be used in tests

                yield return transaction;
            }
        }
    }

    [Fact]
    public void Mine_WithItemExcluder_ExcludesItems()
    {
        // Arrange
        _itemExcluderMock
            .Setup(x => x.ShouldExclude(_itemA))
            .Returns(true);
        _itemExcluderMock
            .Setup(x => x.ShouldExclude(It.IsIn(_itemB, _itemC)))
            .Returns(false);
        var expected = new List<AssociationRule>()
        {
            new(_itemB, _itemC, 4, 3, 2, 6),
            new(_itemC, _itemB, 3, 4, 2, 6),
        };

        // Act
        var actual = _miner.Mine(_transactions, new(0, 0, itemExclusionRules: _itemExclusionRules));

        // Assert
        AssertEqualAssociationRules(expected, actual);
    }

    [Fact]
    public void Mine_WithItemConverter_ReplacesItems()
    {
        // Arrange
        var group = _group;

        _itemConverterMock
            .Setup(x => x.TryConvert(It.IsIn(_itemA, _itemB), out group))
            .Returns(true);
        _itemConverterMock
            .Setup(x => x.TryConvert(_itemC, out It.Ref<Item>.IsAny))
            .Returns(false);

        var expected = new List<AssociationRule>()
        {
            new(_group, _itemC, 6, 3, 3, 6),
            new(_itemC, _group, 3, 6, 3, 6),
        };

        // Act
        var actual = _miner.Mine(_transactions, new(0, 0, _itemConversionRules));

        // Assert
        AssertEqualAssociationRules(expected, actual);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 0)]
    [InlineData(0, 0.5)]
    [InlineData(0.5, 0.5)]
    public void Mine_WithMinSupportAndMinConfidence_ReturnsRules(
        double minSupport, double minConfidence)
    {
        // Arrange
        var expected = GetAllAssociationRules()
            .Where(r => r.Support >= minSupport && r.Confidence >= minConfidence)
            .ToList();

        // Act
        var actual = _miner.Mine(_transactions, new(minSupport, minConfidence));

        // Assert
        AssertEqualAssociationRules(expected, actual);
    }

    [Fact]
    public void Mine_WithDegreeOfParallelism_ReturnsAllRules()
    {
        // Arrange
        var expected = GetAllAssociationRules();

        // Act
        var actual = _miner.Mine(_transactions, new(0, 0, degreeOfParallelism: expected.Count));

        // Assert
        AssertEqualAssociationRules(expected, actual);
    }

    [Fact]
    public void Mine_WithAlreadyCanceledToken_ThrowsOperationCancelledException()
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        cancellationTokenSource.Cancel();

        // Act & Assert
        Assert.ThrowsAny<OperationCanceledException>(
            () => _miner.Mine(_transactions, new(0, 0), cancellationToken));
    }

    [Fact(Timeout = 100)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task Mine_CancelTokenDuringMining_ThrowsOperationCancelledException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Act & Assert
        Assert.ThrowsAny<OperationCanceledException>(
            () => _miner.Mine(GetTransactions(), new(0, 0), cancellationToken));

        IEnumerable<Item[]> GetTransactions()
        {
            yield return [_itemA, _itemB];

            cancellationTokenSource.Cancel();

            yield return [_itemB, _itemC];
        }
    }

    private static void AssertEqualAssociationRules(IReadOnlyCollection<AssociationRule> expected, IReadOnlyCollection<AssociationRule> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        var equalityComparer = EqualityComparer<AssociationRule>.Create((x, y) =>
            x!.LeftHandSide.Item.Name == y!.LeftHandSide.Item.Name &&
            x.RightHandSide.Item.Name == y.RightHandSide.Item.Name &&
            x.LeftHandSide.Count == y.LeftHandSide.Count &&
            x.RightHandSide.Count == y.RightHandSide.Count &&
            x.PairCount == y.PairCount &&
            x.TransactionCount == y.TransactionCount);

        Assert.All(expected, e => Assert.Contains(e, actual, equalityComparer));
    }

    private List<AssociationRule> GetAllAssociationRules() =>
        [
            new(_itemA, _itemB, 5, 4, 3, 6),
            new(_itemB, _itemA, 4, 5, 3, 6),
            new(_itemA, _itemC, 5, 3, 2, 6),
            new(_itemC, _itemA, 3, 5, 2, 6),
            new(_itemB, _itemC, 4, 3, 2, 6),
            new(_itemC, _itemB, 3, 4, 2, 6),
        ];
}
