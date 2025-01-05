namespace MarketBasketAnalysis.Server.Application.Exceptions;

public class AssociationRuleSetException : Exception
{
    public AssociationRuleSetException(string message) : base(message)
    {
    }

    public AssociationRuleSetException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
