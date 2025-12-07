using MarketBasketAnalysis.Analysis;
using Moq;

namespace MarketBasketAnalysis.UnitTests;

public class MaximalCliqueFinderTests
{
    [Fact]
    public void Find_WithNullAssociationRules_ThrowsArgumentNullException()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out _);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => finder.Find(null, new(2, 5)));
    }

    [Fact]
    public void Find_WithNullParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out _);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => finder.Find([], null));
    }

    [Fact]
    public void Find_AssociationRulesContainsNullRule_ThrowsArgumentException()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out _);
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => finder.Find([null], parameters));
    }

    [Fact]
    public void Find_AssociationRulesContainsDuplicateRules_ThrowsArgumentException()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out _);
        var rule = GetAssociationRules()[0];
        var associationRules = new List<AssociationRule> { rule, rule };
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => finder.Find(associationRules, parameters));
    }

    [Fact]
    public void Find_AssociationRulesIsEmpty_NotInvokesAlgorithm()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = Array.Empty<AssociationRule>();
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act
        finder.Find(rules, parameters);

        // Assert
        VerifyAlgorithmMock<byte>(algorithmMock, isAlgorithmInvoked: false);
        VerifyAlgorithmMock<ushort>(algorithmMock, isAlgorithmInvoked: false);
        VerifyAlgorithmMock<int>(algorithmMock, isAlgorithmInvoked: false);
    }

    [Fact]
    public void Find_CanceledBeforeExecution_ThrowsOperationCanceledException()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5);
        using var cancellationTokenSource = new CancellationTokenSource();

        // Act
        cancellationTokenSource.Cancel();

        // Asset
        Assert.Throws<OperationCanceledException>(() =>
            finder.Find(rules, parameters, cancellationTokenSource.Token).ToList());
        VerifyAlgorithmMock<byte>(algorithmMock, isAlgorithmInvoked: false);
        VerifyAlgorithmMock<ushort>(algorithmMock, isAlgorithmInvoked: false);
        VerifyAlgorithmMock<int>(algorithmMock, isAlgorithmInvoked: false);
    }

    [Fact(Timeout = 100)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task Find_CanceledDuringExecution_ThrowsOperationCanceledException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5);
        using var cancellationTokenSource = new CancellationTokenSource();

        algorithmMock.Setup(
            a => a.Find(
                It.IsAny<IReadOnlyDictionary<byte, HashSet<byte>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            // ReSharper disable once AccessToDisposedClosure
            .Callback(() => cancellationTokenSource.Cancel())
            .Returns([new MaximalClique<byte>([0, 1])]);

        // Act & Assert
        Assert.Throws<OperationCanceledException>(() =>
            finder.Find(rules, parameters, cancellationTokenSource.Token).ToList());
    }

    [Fact]
    public void Find_IgnoreOneWayLinksIsTrue_ExcludesOneWayLinks()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5, true);

        // Act
        _ = finder.Find(rules, parameters).ToList();

        // Assert
        VerifyAlgorithmMock<byte>(
            algorithmMock,
            new()
            {
                { 0, [1] },
                { 1, [0] },
            });
    }

    [Fact]
    public void Find_IgnoreOneWayLinksIsFalse_IncludesOneWayLinks()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act
        _ = finder.Find(rules, parameters).ToList();

        // Assert
        VerifyAlgorithmMock<byte>(
            algorithmMock,
            new()
            {
                { 0, [1, 2] },
                { 1, [0, 2] },
                { 2, [0, 1] },
            });
    }

    [Fact]
    public void Find_ParametersProvided_PassesSpecifiedCliqueSizeToAlgorithm()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act
        _ = finder.Find(rules, parameters).ToList();

        // Assert
        VerifyAlgorithmMock<byte>(
            algorithmMock,
            expectedMinCliqueSize: parameters.MinCliqueSize,
            expectedMaxCliqueSize: parameters.MaxCliqueSize);
    }

    [Theory]
    [InlineData(2, true, false, false)]
    [InlineData(256, true, false, false)]
    [InlineData(257, false, true, false)]
    [InlineData(65536, false, true, false)]
    [InlineData(65537, false, false, true)]
    public void Find_DifferentItemsCount_SelectsAppropriateVertexType(
        int itemsCount, bool isVertexByte, bool isVertexUshort, bool isVertexInt)
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GenerateAssociationRules(itemsCount);
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        // Act
        _ = finder.Find(rules, parameters).ToList();

        // Assert
        VerifyAlgorithmMock<byte>(algorithmMock, isAlgorithmInvoked: isVertexByte);
        VerifyAlgorithmMock<ushort>(algorithmMock, isAlgorithmInvoked: isVertexUshort);
        VerifyAlgorithmMock<int>(algorithmMock, isAlgorithmInvoked: isVertexInt);
    }

    [Fact]
    public void Find_CliquesFound_ReturnsCorrectAssociationRules()
    {
        // Arrange
        var finder = GetMaximalCliqueFinder(out var algorithmMock);
        var rules = GetAssociationRules();
        var parameters = new MaximalCliqueFindingParameters(2, 5);

        algorithmMock.Setup(
            a => a.Find(
                It.IsAny<IReadOnlyDictionary<byte, HashSet<byte>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .Returns([new MaximalClique<byte>([0, 1, 2])]);

        // Act
        var maximalCliques = finder.Find(rules, parameters).ToList();

        // Assert
        Assert.Single(maximalCliques);
        Assert.Equivalent(rules, maximalCliques[0], true);
    }

    private static MaximalCliqueFinder GetMaximalCliqueFinder(out Mock<IMaximalCliqueAlgorithm> algorithmMock)
    {
        algorithmMock = new Mock<IMaximalCliqueAlgorithm>();

        return new MaximalCliqueFinder(algorithmMock.Object);
    }

    private static List<AssociationRule> GetAssociationRules()
    {
        var itemA = new Item(1, "A", false);
        var itemB = new Item(2, "B", false);
        var itemC = new Item(3, "C", false);

        return
        [
            CreateAssociationRule(itemA, itemB),
            CreateAssociationRule(itemB, itemA),
            CreateAssociationRule(itemA, itemC),
            CreateAssociationRule(itemB, itemC),
        ];

        static AssociationRule CreateAssociationRule(Item lhsItem, Item rhsItem) =>
            new(lhsItem, rhsItem, 1, 1, 1, 10);
    }

    private static List<AssociationRule> GenerateAssociationRules(int itemsCount)
    {
        var associationRules = new List<AssociationRule>();

        for (var i = 1; i <= itemsCount - 1; i++)
        {
            var lhsItem = new Item(i, $"{i}", false);
            var rhsItem = new Item(i + 1, $"{i + 1}", false);

            associationRules.Add(new(lhsItem, rhsItem, 1, 1, 1, 1));
        }

        return associationRules;
    }

    private static void VerifyAlgorithmMock<TVertex>(
        Mock<IMaximalCliqueAlgorithm> algorithmMock,
        Dictionary<TVertex, HashSet<TVertex>>? expectedAdjacencyList = null,
        int? expectedMinCliqueSize = null,
        int? expectedMaxCliqueSize = null,
        bool isAlgorithmInvoked = true)
        where TVertex : struct
    {
        algorithmMock.Verify(
            a => a.Find(
                It.IsAny<IReadOnlyDictionary<TVertex, HashSet<TVertex>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()),
            isAlgorithmInvoked ? Times.Once() : Times.Never());

        if (!isAlgorithmInvoked)
        {
            return;
        }

        var invocation = algorithmMock.Invocations[0];

        if (expectedAdjacencyList != null)
        {
            var actualAdjacencyList = (IReadOnlyDictionary<TVertex, HashSet<TVertex>>)invocation.Arguments[0];

            Assert.Equivalent(expectedAdjacencyList, actualAdjacencyList, true);
        }

        if (expectedMinCliqueSize != null)
        {
            var actualMinCliqueSize = (int)invocation.Arguments[1];

            Assert.Equal(expectedMinCliqueSize, actualMinCliqueSize);
        }

        if (expectedMaxCliqueSize != null)
        {
            var actualMaxCliqueSize = (int)invocation.Arguments[2];

            Assert.Equal(expectedMaxCliqueSize, actualMaxCliqueSize);
        }
    }
}
