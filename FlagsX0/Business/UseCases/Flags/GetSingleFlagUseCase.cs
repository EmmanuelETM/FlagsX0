using FlagsX0.Business.Mappers;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using FlagsX0.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases.Flags;

public class GetSingleFlagUseCase(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result<FlagDto>> Execute(string flagName)
    {
        return await GetFromDb(flagName).Map(result => result.ToDto());
    }

    private async Task<Result<FlagEntity>> GetFromDb(string flagName)
    {
        return await _dbContext.Flags
            .Where(flag =>
                EF.Functions.Like(flag.Name.ToLower(), flagName.ToLower()))
            .AsNoTracking()
            .SingleAsync();
    }
}