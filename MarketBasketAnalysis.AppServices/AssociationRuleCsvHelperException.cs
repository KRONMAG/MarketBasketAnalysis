using System;

namespace MarketBasketAnalysis.AppServices
{
    public class AssociationRuleCsvHelperException : Exception
    {
        public AssociationRuleCsvHelperException() : base()
        {
        }

        public AssociationRuleCsvHelperException(string? message) : base(message)
        {
        }

        public AssociationRuleCsvHelperException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
