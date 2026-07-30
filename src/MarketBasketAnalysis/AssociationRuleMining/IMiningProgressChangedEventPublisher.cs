namespace MarketBasketAnalysis.AssociationRuleMining
{
    internal interface IMiningProgressChangedEventPublisher
    {
        void Publish(double progress);
    }
}