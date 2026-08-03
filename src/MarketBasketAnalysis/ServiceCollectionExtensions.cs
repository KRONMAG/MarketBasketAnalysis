using System;
using MarketBasketAnalysis.AssociationRuleAnalysis;
using MarketBasketAnalysis.AssociationRuleAnalysis.Contracts;
using MarketBasketAnalysis.AssociationRuleMining;
using MarketBasketAnalysis.AssociationRuleMining.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MarketBasketAnalysis
{
    /// <summary>
    /// Provides extension methods for registering Market Basket Analysis services in a dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Market Basket Analysis services.
        /// </summary>
        /// <param name="services">The service collection to add the Market Basket Analysis services to.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="services"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method registers implementations of <see cref="IMinerFactory">IMinerFactory</see>
        /// and <see cref="IMaximalCliqueFinder">IMaximalCliqueFinder</see> as singletons.
        /// </remarks>
        public static IServiceCollection AddMarketBasketAnalysis(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IMinerFactory, MinerFactory>();
            services.AddSingleton<IMaximalCliqueFinder, MaximalCliqueFinder>();
            services.AddSingleton<IMaximalCliqueAlgorithm, TomitaAlgorithm>();

            return services;
        }
    }
}