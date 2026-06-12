using MarketBasketAnalysis.Mining;
using Moq;
#pragma warning disable S2699 // Tests should include assertions
#pragma warning disable CA1062 // Validate arguments of public methods

namespace MarketBasketAnalysis.UnitTests;

public class AsyncMinerTests : BaseMinerTests
{
    [Fact]
    public async Task WithSyncronizationContext_DoesntUseSyncronizationContext()
    {
        // Arrange
        var contextMock = new Mock<SynchronizationContext>();
        var isContextUsed = false;

        contextMock
            .Setup(c => c.Send(It.IsAny<SendOrPostCallback>(), It.IsAny<object>()))
            .Callback((SendOrPostCallback c, object s) =>
            {
                isContextUsed = true;
                c(s);
            });
        contextMock
            .Setup(c => c.Post(It.IsAny<SendOrPostCallback>(), It.IsAny<object>()))
            .Callback((SendOrPostCallback c, object s) =>
            {
                isContextUsed = true;
                Task.Run(() => c(s));
            });

        var sourceContext = SynchronizationContext.Current;

        try
        {
            // Arrange
            SynchronizationContext.SetSynchronizationContext(contextMock.Object);

            // Act
            await MineAsync(Miner, Transactions, new(0, 0));

            // Assert
            Assert.False(isContextUsed);
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(sourceContext);
        }
    }

    protected override Task<IReadOnlyCollection<AssociationRule>> MineAsync(
        IMiner miner,
        IEnumerable<IReadOnlyList<Item>> transactions,
        MiningParameters parameters,
        CancellationToken cancellationToken = default) =>
        miner.MineAsync(transactions.ToAsyncEnumerable(), parameters, cancellationToken);
}