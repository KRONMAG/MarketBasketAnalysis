namespace MarketBasketAnalysis.Infrastructure.DataTransferObjects
{
    public class AssociationRuleDTO
    {
        public string LeftHandSideName { get; set; } = null!;

        public string RightHandSideName { get; set; } = null!;

        public int LeftHandSideCount { get; set; }

        public int RightHandSideCount { get; set; }

        public int Count { get; set; }

        public double Support { get; set; }

        public double Confidence { get; set; }

        public double Lift { get; set; }

        public double Conviction { get; set; }

        public double AbsoluteAssociationCoefficient { get; set; }

        public double AbsoluteContingencyCoefficient { get; set; }

        public double ChiSquareStatistic { get; set; }
    }
}
