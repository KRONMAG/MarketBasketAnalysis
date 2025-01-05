namespace MarketBasketAnalysis.Server.Application.Exceptions;

public class AssociationRuleSetLoadException(string message, Exception innerException)
    : AssociationRuleSetException(message, innerException);