#pragma warning disable

namespace MarketBasketAnalysis.AssociationRuleMining.Contracts
{
    public interface IMiningProgressChangedEventPublisher
    {
        void Publish(double progress);
    }
}