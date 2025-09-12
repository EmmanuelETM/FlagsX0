using FlagsX0.Business.Mappers;
using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using FlagsX0.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases;

public class GetSingleFlagUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<Result<FlagDto>> Execute(string flagName)
    {
        return await GetFromDb(flagName).Map(result => result.ToDto());
    }

    private async Task<Result<FlagEntity>> GetFromDb(string flagName)
    {
        return await _dbContext.Flags
            .Where(flag => flag.UserId == _userDetails.UserId &&
                           EF.Functions.Like(flag.Name.ToLower(), flagName.ToLower()))
            .AsNoTracking()
            .SingleAsync();
    }
}