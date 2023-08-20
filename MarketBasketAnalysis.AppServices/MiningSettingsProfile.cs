using MarketBasketAnalysis.DomainModel.Mining;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.ContractsLight;
using System.Linq;

namespace MarketBasketAnalysis.AppServices
{
    public class MiningSettingsProfile
    {
        #region Fields and Properties

        private string _name;
        private double _minSupport;
        private double _minConfidence;
        private IReadOnlyCollection<ItemExclusionRule> _itemExclusionRules;
        private IReadOnlyCollection<ItemConversionRule> _itemConversionRules;

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            [MemberNotNull(nameof(_name))]
            set
            {
                Contract.AssertNotNullOrWhiteSpace(value);

                _name = value;
            }
        }

        public string? Description { get; set; }

        public double MinSupport
        {
            get => _minSupport;
            set
            {
                Contract.Requires(value >= 0);

                _minSupport = value;
            }
        }

        public double MinConfidence
        {
            get => _minConfidence;
            set
            {
                Contract.Requires(value >= 0);

                _minConfidence = value;
            }
        }

        public IReadOnlyCollection<ItemExclusionRule> ItemExclusionRules
        {
            get => _itemExclusionRules;
            [MemberNotNull(nameof(_itemExclusionRules))]
            set
            {
                Contract.RequiresNotNull(value);
                Contract.Requires(value.Distinct().Count() == value.Count);

                _itemExclusionRules = value.ToList();
            }
        }

        public IReadOnlyCollection<ItemConversionRule> ItemConversionRules
        {
            get => _itemConversionRules;
            [MemberNotNull(nameof(_itemConversionRules))]
            set
            {
                Contract.RequiresNotNull(value);
                Contract.Requires(value.Distinct().Count() == value.Count);

                _itemConversionRules = value.ToList();
            }
        }

        #endregion Fields and Properties

        #region Constructors

        public MiningSettingsProfile(int id, string name)
        {
            Contract.RequiresNotNullOrWhiteSpace(name);

            Id = id;
            Name = name;
            MinSupport = 0.001;
            MinConfidence = 0.01;
            ItemExclusionRules = new List<ItemExclusionRule>();
            ItemConversionRules = new List<ItemConversionRule>();
        }

        #endregion Constructors
    }
}