using MarketBasketAnalysis.Common.Protos;
using MarketBasketAnalysis.Server.Application.Exceptions;
using MarketBasketAnalysis.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MarketBasketAnalysis.Server.Application.Services;

public class AssociationRuleSetInfoLoader : IAssociationRuleSetInfoLoader
{
    #region Fields and Properties

    private readonly MarketBasketAnalysisDbContext _context;

    #endregion

    #region Constructors

    public AssociationRuleSetInfoLoader(MarketBasketAnalysisDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    #endregion

    #region Methods

    public async Task<List<AssociationRuleSetInfoMessage>> LoadAsync(CancellationToken token = default)
    {
        try
        {
            return await _context.AssociationRuleSets
                .AsNoTracking()
                .Where(e => e.IsAvailable)
                .Select(e => new AssociationRuleSetInfoMessage
                {
                    Name = e.Name,
                    Description = e.Description,
                    TransactionCount = e.TransactionCount
                })
                .ToListAsync(token);
        }
        catch (DbException e)
        {
            throw new AssociationRuleSetLoadException(
                "Unexpected error occurred while loading association rule set info.", e);
        }
    }

    #endregion
}