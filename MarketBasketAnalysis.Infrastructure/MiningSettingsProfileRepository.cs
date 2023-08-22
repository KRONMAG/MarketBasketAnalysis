using AutoMapper;
using MarketBasketAnalysis.AppServices;
using MarketBasketAnalysis.Infrastructure.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBasketAnalysis.Infrastructure;

public class MiningSettingsProfileRepository : IMiningSettingsProfileRepository
{
    #region Fields and Properties

    private readonly ApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    #endregion Fields and Properties

    #region Constructors

    public MiningSettingsProfileRepository(ApplicationContext applicationContext, IMapper mapper)
    {
        Contract.RequiresNotNull(applicationContext);
        Contract.RequiresNotNull(mapper);

        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    #endregion Constructors

    #region Methods

    public async Task<IReadOnlyCollection<string>> GetProfileNamesAsync()
    {
        try
        {
            return await _applicationContext.MiningSettingsProfiles
                .Select(item => item.Name)
                .ToListAsync()
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new RepositoryException(Messages.MiningSettingsProfileRepository_OperationFailed, e);
        }
    }

    public async Task<MiningSettingsProfile> GetAsync(string profileName)
    {
        try
        {
            var profileDTO = await _applicationContext.MiningSettingsProfiles
                .Include(profile => profile.ItemConversionRules)
                .Include(profile => profile.ItemExclusionRules)
                .FirstOrDefaultAsync(profileDTO => profileDTO.Name == profileName)
                .ConfigureAwait(false);

            if (profileDTO == null)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    Messages.MiningSettingsProfileRepository_ProfileNotFound, profileName);

                throw new RepositoryException(message);
            }

            _applicationContext.Entry(profileDTO).State = EntityState.Detached;

            return _mapper.Map<MiningSettingsProfile>(profileDTO);
        }
        catch (Exception e) when (e is not RepositoryException)
        {
            throw new RepositoryException(Messages.MiningSettingsProfileRepository_OperationFailed, e);
        }
    }

    public async Task AddAsync(MiningSettingsProfile profile)
    {
        Contract.RequiresNotNull(profile);

        try
        {
            if (await ProfileExists(profile.Name).ConfigureAwait(false))
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    Messages.MiningSettingsProfileRepository_FailedToAddProfile, profile.Name);

                throw new RepositoryException(message);
            }

            var profileDto = _mapper.Map<MiningSettingsProfileDTO>(profile);

            await _applicationContext.MiningSettingsProfiles.AddAsync(profileDto)
                .ConfigureAwait(false);

            await _applicationContext.SaveChangesAsync()
                .ConfigureAwait(false);

            _applicationContext.Entry(profileDto).State = EntityState.Detached;
        }
        catch (Exception e) when (e is not RepositoryException)
        {
            throw new RepositoryException(Messages.MiningSettingsProfileRepository_OperationFailed, e);
        }
    }

    public async Task RemoveAsync(string profileName)
    {
        Contract.RequiresNotNull(profileName);

        try
        {
            if (!await ProfileExists(profileName).ConfigureAwait(false))
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                    Messages.MiningSettingsProfileRepository_FailedToRemoveProfile, profileName);

                throw new RepositoryException(message);
            }

            await _applicationContext.MiningSettingsProfiles
                .Where(profile => profile.Name == profileName)
                .ExecuteDeleteAsync()
                .ConfigureAwait(false);

            await _applicationContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }
        catch (Exception e) when (e is not RepositoryException)
        {
            throw new RepositoryException(Messages.MiningSettingsProfileRepository_OperationFailed, e);
        }
    }

    private async Task<bool> ProfileExists(string profileName) =>
        await _applicationContext.MiningSettingsProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(profile => profile.Name == profileName)
            .ConfigureAwait(false) != null;

    #endregion Methods
}