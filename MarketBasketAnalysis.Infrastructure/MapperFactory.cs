using AutoMapper;
using MarketBasketAnalysis.AppServices;
using MarketBasketAnalysis.DomainModel;
using MarketBasketAnalysis.DomainModel.Mining;
using MarketBasketAnalysis.Infrastructure.DataTransferObjects;

namespace MarketBasketAnalysis.Infrastructure
{
    public class MapperFactory
    {
#pragma warning disable CA1822 // Mark members as static
        public IMapper Create()
#pragma warning restore CA1822 // Mark members as static
        {
            var configuration = new MapperConfiguration(o =>
            {
                o.CreateMap<AssociationRule, AssociationRuleDTO>();
                o.CreateMap<AssociationRuleDTO, AssociationRule>()
                    .ConvertUsing(src => new AssociationRule(src.LeftHandSideName, src.RightHandSideName,
                        src.LeftHandSideCount, src.RightHandSideCount, src.Count, src.Support));

                o.CreateMap<ItemExclusionRule, ItemExclusionRuleDTO>();
                o.CreateMap<ItemExclusionRuleDTO, ItemExclusionRule>()
                    .ConvertUsing(src => new ItemExclusionRule(src.ItemPattern, src.ExactMatch, src.IgnoreCase));

                o.CreateMap<ItemConversionRule, ItemConversionRuleDTO>();
                o.CreateMap<ItemConversionRuleDTO, ItemConversionRule>()
                    .ConvertUsing(src => new ItemConversionRule(src.Item, src.Group));

                o.CreateMap<MiningSettingsProfile, MiningSettingsProfileDTO>();
                o.CreateMap<MiningSettingsProfileDTO, MiningSettingsProfile>()
                    .ConstructUsing(src => new MiningSettingsProfile(src.Name));
            });

            return configuration.CreateMapper();
        }
    }
}
