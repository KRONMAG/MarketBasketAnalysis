﻿using MarketBasketAnalysis.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketBasketAnalysis.Server.API.Extensions;

public static class ServicesExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<MarketBasketAnalysisDbContext>(optionsBuilder =>
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection")),
            optionsLifetime: ServiceLifetime.Singleton);
        services.AddDbContextFactory<MarketBasketAnalysisDbContext>(optionsBuilder =>
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
}