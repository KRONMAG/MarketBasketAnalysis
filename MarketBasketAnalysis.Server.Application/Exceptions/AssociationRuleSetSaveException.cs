namespace MarketBasketAnalysis.Server.Application.Exceptions;

public class AssociationRuleSetSaveException : AssociationRuleSetException
{
    public AssociationRuleSetSaveException(string message) : base(message)
    {
    }

    public AssociationRuleSetSaveException(string message, Exception innerException) : base(message, innerException)
    {
    }
}