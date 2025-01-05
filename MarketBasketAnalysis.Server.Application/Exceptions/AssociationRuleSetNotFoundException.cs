namespace MarketBasketAnalysis.Server.Application.Exceptions;

public class AssociationRuleSetNotFoundException(string associationRuleSetName)
    : AssociationRuleSetException($"Association rule set with name \"{associationRuleSetName}\" not found.");