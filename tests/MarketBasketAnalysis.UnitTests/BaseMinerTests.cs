// Ignore Spelling: Excluder

using MarketBasketAnalysis.AssociationRuleMining;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using MarketBasketAnalysis.Models;
using Moq;

namespace MarketBasketAnalysis.UnitTests;

public abstract class BaseMinerTests
{
    private readonly Item _itemA;
    private readonly Item _itemB;
    private readonly Item _itemC;
    private readonly Item _group;
    private readonly Mock<IItemConverter> _itemConverterMock;
    private readonly Mock<IItemExcluder> _itemExcluderMock;
    private readonly List<ItemExclusionRule> _itemExclusionRules;
    private readonly List<ItemConversionRule> _itemConversionRules;

    protected IMiner Miner { get; }

    protected IEnumerable<IReadOnlyList<Item>> Transactions { get; }

    protected abstract Task<IReadOnlyCollection<AssociationRule>> MineAsync(
        IMiner miner,
        IEnumerable<IReadOnlyList<Item>> transactions,
        MiningParameters parameters,
        CancellationToken cancellationToken = default);

    protected BaseMinerTests()
    {
        _itemA = new(1, "A");
        _itemB = new(2, "B");
        _itemC = new(3, "C");
        _group = new(4, "Group", true);

        Transactions =
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

        Miner = new Miner(
            new SearchForFrequentItemsStep(_ => _itemExcluderMock.Object, _ => _itemConverterMock.Object),
            new SearchForItemsetsStep(_ => _itemConverterMock.Object),
            new GenerateAssociationRulesStep());
    }

    [Fact]
    public async Task InvalidArguments_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => MineAsync(Miner, null!, new(0, 0)));
        await Assert.ThrowsAsync<ArgumentNullException>(() => MineAsync(Miner, [], null!));
        await Assert.ThrowsAnyAsync<Exception>(() => MineAsync(Miner, [null!], new(0, 0)));
    }

    [Fact]
    public async Task WithEmptyTransactions_ReturnsNoAssociationRules()
    {
        // Act
        var actual = await MineAsync(Miner, [], new(0, 0));

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public async Task WithOneItemTransactions_ReturnsNoAssociationRules()
    {
        // Act
        var actual = await MineAsync(Miner, [[_itemA]], new(0, 0));

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public async Task StageChanged_RaisesEvents()
    {
        // Arrange
        var stages = new List<MiningStage>();

        Miner.MiningStageChanged += (_, e) => stages.Add(e.Stage);

        // Act
        await MineAsync(Miner, Transactions, new(0, 0));

        // Assert
        Assert.Equal(
            [MiningStage.FrequentItemSearch, MiningStage.ItemsetSearch, MiningStage.AssociationRuleGeneration],
            stages);
    }

    [Fact]
    public async Task ProgressChanged_RaisesEvents()
    {
        // Arrange
        var progressValues = new List<double>();

        Miner.MiningProgressUpdated += (_, e) => progressValues.Add(e.Progress);

        // Act
        await MineAsync(Miner, GenerateTransactions(), new(0, 0));

        // Assert
        Assert.NotEmpty(progressValues);
        Assert.All(progressValues, v => Assert.InRange(v, 0, 100));

        IEnumerable<IReadOnlyList<Item>> GenerateTransactions()
        {
            foreach (var transaction in Transactions)
            {
#pragma warning disable S2925 // "Thread.Sleep" should not be used in tests
                Thread.Sleep(100);
#pragma warning restore S2925 // "Thread.Sleep" should not be used in tests

                yield return transaction;
            }
        }
    }

    [Fact]
    public async Task WithItemExcluder_ExcludesItems()
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
        var actual = await MineAsync(Miner, Transactions, new(0, 0, itemExclusionRules: _itemExclusionRules));

        // Assert
        Assert.Equivalent(expected, actual, true);
    }

    [Fact]
    public async Task WithItemConverter_ReplacesItems()
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
        var actual = await MineAsync(Miner, Transactions, new(0, 0, _itemConversionRules));

        // Assert
        Assert.Equivalent(expected, actual, true);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0.5, 0)]
    [InlineData(0, 0.5)]
    [InlineData(0.5, 0.5)]
    public async Task WithMinSupportAndMinConfidence_ReturnsRules(
        double minSupport, double minConfidence)
    {
        // Arrange
        var expected = GetAllAssociationRules()
            .Where(r => r.Support >= minSupport && r.Confidence >= minConfidence)
            .ToList();

        // Act
        var actual = await MineAsync(Miner, Transactions, new(minSupport, minConfidence));

        // Assert
        Assert.Equivalent(expected, actual, true);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public async Task WithDegreeOfParallelism_ReturnsAllRules(int degreeOfParallelism)
    {
        // Arrange
        var expected = GetAllAssociationRules();

        // Act
        var actual = await MineAsync(Miner, Transactions, new(0, 0, degreeOfParallelism: degreeOfParallelism));

        // Assert
        Assert.Equivalent(expected, actual, true);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(10, 1)]
    [InlineData(10, 5)]
    [InlineData(10, 10)]
    public async Task WithStatePartitionCount_ReturnsAllRules(
        int degreeOfParallelism, int statePartitionCount)
    {
        // Arrange
        var parameters = new MiningParameters(
            0, 0, degreeOfParallelism: degreeOfParallelism, statePartitionCount: statePartitionCount);
        var expected = GetAllAssociationRules();

        // Act
        var actual = await MineAsync(Miner, Transactions, parameters);

        // Assert
        Assert.Equivalent(expected, actual, true);
    }

    [Fact]
    public async Task WithMineAgain_MinerBehaviorIsRepeatable()
    {
        // Act
        var associationRules1 = await MineAsync(Miner, Transactions, new(0, 0));
        var associationRules2 = await MineAsync(Miner, Transactions, new(0, 0));

        // Assert
        Assert.Equivalent(associationRules1, associationRules2, true);
    }

    [Fact]
    public async Task WithAlreadyCanceledToken_ThrowsOperationCancelledException()
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        await cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => MineAsync(Miner, Transactions, new(0, 0), cancellationToken));
    }

    [Fact(Timeout = 100)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task CancelTokenDuringMining_ThrowsOperationCancelledException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // Arrange
        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => MineAsync(Miner, GetTransactions(), new(0, 0), cancellationToken));

        IEnumerable<Item[]> GetTransactions()
        {
            yield return [_itemA, _itemB];

            cancellationTokenSource.Cancel();

            yield return [_itemB, _itemC];
        }
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
