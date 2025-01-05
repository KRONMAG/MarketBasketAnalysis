namespace MarketBasketAnalysis.Server.Application.Exceptions;

[Serializable]
public class AssociationRuleSetValidationException(string message) : AssociationRuleSetException(message);