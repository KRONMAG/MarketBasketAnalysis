using MarketBasketAnalysis.Server.Application.Exceptions;
using MarketBasketAnalysis.Server.Application.Extensions;
using MarketBasketAnalysis.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketBasketAnalysis.Server.Application.Services;

public sealed class AssociationRuleSetRemover : IAssociationRuleSetRemover
{
    #region Fields and Properties

    private readonly MarketBasketAnalysisDbContext _context;

    #endregion

    #region Constructors

    public AssociationRuleSetRemover(MarketBasketAnalysisDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
    }

    #endregion

    #region Methods

    public async Task RemoveAsync(string associationRuleSetName, CancellationToken token)
    {
        associationRuleSetName.ValidateAssociationRuleSetName();

        AssociationRuleSet? associationRuleSet = null;

        try
        {
            associationRuleSet = await _context.AssociationRuleSets
                .FirstOrDefaultAsync(e => e.IsAvailable && e.Name == associationRuleSetName, token);

            if (associationRuleSet == null)
                throw new AssociationRuleSetNotFoundException(associationRuleSetName);

            _context.Remove(associationRuleSet);
            await _context.SaveChangesAsync(token);
        }
        catch (Exception e) when (e.IsDbOrDbUpdateException())
        {
            throw new AssociationRuleSetRemoveException(associationRuleSetName, e);
        }
        finally
        {
            if (associationRuleSet != null)
                _context.Entry(associationRuleSet).State = EntityState.Detached;
        }
    }

    #endregion
}