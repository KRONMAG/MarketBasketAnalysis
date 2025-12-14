using MarketBasketAnalysis.Mining;
using Microsoft.Extensions.DependencyInjection;

namespace MarketBasketAnalysis.Examples.Extensions;

public sealed class ExampleHelper
{
    public IMiner CreateMiner()
    {
        var services = new ServiceCollection();

        services.AddMarketBasketAnalysis();

        var serviceProvider = services.BuildServiceProvider();
        var minerFactory = serviceProvider.GetRequiredService<IMinerFactory>();

        return minerFactory.Create();
    }

    public void PrintAssociationRules(IReadOnlyCollection<AssociationRule> associationRules)
    {
        foreach (var associationRule in associationRules)
        {
            Console.WriteLine($"{associationRule}: support {associationRule.Support:f2}, confidence {associationRule.Confidence:f2}");
        }
    }
}
