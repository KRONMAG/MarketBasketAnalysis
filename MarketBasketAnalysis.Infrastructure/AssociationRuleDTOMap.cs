using CsvHelper.Configuration;
using MarketBasketAnalysis.Infrastructure.DataTransferObjects;

namespace MarketBasketAnalysis.Infrastructure
{
    public class AssociationRuleDTOMap : ClassMap<AssociationRuleDTO>
    {
        public AssociationRuleDTOMap()
        {
            Map(m => m.LeftHandSideName).Name("LHS");
            Map(m => m.RightHandSideName).Name("RHS");
            Map(m => m.LeftHandSideCount).Name("LHS count");
            Map(m => m.RightHandSideCount).Name("RHS count");
            Map(m => m.Count).Name("Count");
            Map(m => m.Support).Name("Support");
            Map(m => m.Confidence).Name("Confidence");
            Map(m => m.Lift).Name("Lift");
            Map(m => m.Conviction).Name("Conviction");
            Map(m => m.AbsoluteAssociationCoefficient).Name("Absolute association coefficient");
            Map(m => m.AbsoluteContingencyCoefficient).Name("Absolute contingency coefficient");
            Map(m => m.ChiSquareStatistic).Name("Chi-square statistic");
        }
    }
}
