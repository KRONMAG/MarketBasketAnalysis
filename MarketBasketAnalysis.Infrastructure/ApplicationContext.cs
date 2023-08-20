using MarketBasketAnalysis.Infrastructure.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.ContractsLight;

namespace MarketBasketAnalysis.Infrastructure;

public class ApplicationContext : DbContext
{
    #region Fields and Properties

    private readonly string _connectionString;
    private readonly ILoggerFactory? _loggerFactory;

    public DbSet<MiningSettingsProfileDTO> MiningSettingsProfiles { get; set; }

    public DbSet<ItemConversionRuleDTO> ItemConversionRules { get; set; }

    public DbSet<ItemExclusionRuleDTO> ItemExclusionRules { get; set; }

    #endregion Fields and Properties

    #region Constructors

    public ApplicationContext(string connectionString, ILoggerFactory? loggerFactory = null)
    {
        Contract.RequiresNotNullOrWhiteSpace(connectionString);

        _connectionString = connectionString;
        _loggerFactory = loggerFactory;
    }

    #endregion Constructors

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);

        if (_loggerFactory != null)
            optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemConversionRuleDTO>().HasAlternateKey(r => new { r.Item, r.Group });
        modelBuilder.Entity<ItemExclusionRuleDTO>().HasAlternateKey(r =>
            new { r.ItemPattern, r.ExactMatch, r.IgnoreCase });
        modelBuilder.Entity<MiningSettingsProfileDTO>().Property(p => p.Name).IsRequired();

        base.OnModelCreating(modelBuilder);
    }

    #endregion Methods
}