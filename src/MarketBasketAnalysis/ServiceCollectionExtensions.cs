using System;
using JetBrains.Annotations;
using MarketBasketAnalysis.Analysis;
using MarketBasketAnalysis.Mining;
using Microsoft.Extensions.DependencyInjection;

namespace MarketBasketAnalysis
{
    /// <summary>
    /// Provides extension methods for registering Market Basket Analysis services in a dependency injection container.
    /// </summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all core Market Basket Analysis services in the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the Market Basket Analysis services to.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="services"/> is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method registers the following services as singletons:
        /// <list type="bullet">
        /// <item><description><see cref="IMaximalCliqueAlgorithm"/> (implemented by <see cref="TomitaAlgorithm"/>)</description></item>
        /// <item><description><see cref="IMaximalCliqueFinder"/> (implemented by <see cref="MaximalCliqueFinder"/>)</description></item>
        /// <item><description><see cref="IMinerFactory"/> (implemented by <see cref="MinerFactory"/>)</description></item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddMarketBasketAnalysis(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IMaximalCliqueAlgorithm, TomitaAlgorithm>();
            services.AddSingleton<IMaximalCliqueFinder, MaximalCliqueFinder>();
            services.AddSingleton<IMinerFactory, MinerFactory>();

            return services;
        }
    }
}