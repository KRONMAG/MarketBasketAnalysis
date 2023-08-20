using System;

namespace MarketBasketAnalysis.DomainModel.Mining
{
    public class MinerException : Exception
    {
        public MinerException() : base()
        {
        }

        public MinerException(string? message) : base(message)
        {
        }

        public MinerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
