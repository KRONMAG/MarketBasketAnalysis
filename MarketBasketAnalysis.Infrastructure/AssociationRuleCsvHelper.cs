using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using MarketBasketAnalysis.AppServices;
using MarketBasketAnalysis.DomainModel;
using MarketBasketAnalysis.Infrastructure.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Infrastructure
{
    public class AssociationRuleCsvHelper : IAssociationRuleCsvHelper
    {
        #region Fields and Properties

        private readonly IMapper _mapper;

        #endregion Fields and Properties

        #region Constructors

        public AssociationRuleCsvHelper(IMapper mapper)
        {
            Contract.RequiresNotNull(mapper);

            _mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        public async Task<IReadOnlyCollection<AssociationRule>> ReadAsync(string path)
        {
            Contract.Requires(File.Exists(path));

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            try
            {
                using (var streamReader = new StreamReader(path))
                using (var csvReader = new CsvReader(streamReader, config))
                {
                    csvReader.Context.RegisterClassMap<AssociationRuleDTOMap>();

                    return await csvReader
                        .GetRecordsAsync<AssociationRuleDTO>()
                        .Select(_mapper.Map<AssociationRule>)
                        .ToListAsync()
                        .ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    Messages.AssociationRuleCsvHelper_FailedToReadRulesFromFile, path);

                throw new AssociationRuleCsvHelperException(message, e);
            }
        }

        public async Task WriteAsync(string path, IReadOnlyCollection<AssociationRule> associationRules)
        {
            Contract.Requires(!Path.GetInvalidFileNameChars().Any(path.Contains));
            Contract.RequiresNotNull(associationRules);
            Contract.RequiresForAll(associationRules, item => item != null);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            try
            {
                using (var streamWriter = new StreamWriter(path))
                using (var csvWriter = new CsvWriter(streamWriter, config))
                {
                    csvWriter.Context.RegisterClassMap<AssociationRuleDTOMap>();

                    await csvWriter
                        .WriteRecordsAsync(associationRules.Select(_mapper.Map<AssociationRuleDTO>))
                        .ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    Messages.AssociationRuleCsvHelper_FailedToWriteRulesToFile, path);

                throw new AssociationRuleCsvHelperException(message, e);
            }
        }

        #endregion Methods
    }
}
