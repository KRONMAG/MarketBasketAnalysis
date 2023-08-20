using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.Infrastructure;

public class DesignTimeApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        Contract.RequiresNotNull(args);

        return new ApplicationContext("Data Source=application.db");
    }
}