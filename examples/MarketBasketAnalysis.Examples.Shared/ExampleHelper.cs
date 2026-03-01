using MarketBasketAnalysis.Analysis;
using MarketBasketAnalysis.Mining;
using Microsoft.Extensions.DependencyInjection;

namespace MarketBasketAnalysis.Examples;

public static class ExampleHelper
{
    public static IMiner CreateMiner()
    {
        var minerFactory = GetService<IMinerFactory>();

        return minerFactory.Create();
    }

    public static IMaximalCliqueFinder CreateMaximalCliqueFinder() =>
        GetService<IMaximalCliqueFinder>();

    public static void Print(IEnumerable<AssociationRule> associationRules)
    {
        ArgumentNullException.ThrowIfNull(associationRules);

        foreach (var associationRule in associationRules)
        {
            Console.WriteLine($"{associationRule}: support {associationRule.Support:f2}, confidence {associationRule.Confidence:f2}");
        }
    }

    public static void Print(IEnumerable<IReadOnlyCollection<AssociationRule>> maximalCliques)
    {
        ArgumentNullException.ThrowIfNull(maximalCliques);

        var number = 1;

        foreach (var maximalClique in maximalCliques)
        {
            Console.WriteLine($"Maximal clique {number}:");

            foreach (var associationRule in maximalClique)
            {
                Console.WriteLine(associationRule);
            }

            Console.WriteLine();

            number++;
        }
    }

    private static TService GetService<TService>()
        where TService : notnull
    {
        var services = new ServiceCollection();

        services.AddMarketBasketAnalysis();

        using var serviceProvider = services.BuildServiceProvider();

        return serviceProvider.GetRequiredService<TService>();
    }
}
