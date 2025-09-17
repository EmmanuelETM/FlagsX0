using FlagsX0.Business.Mappers;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using FlagsX0.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases.Flags;

public class UpdateFlagUseCase(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result<FlagDto>> Execute(FlagDto flag)
    {
        return await VerifyExisting(flag)
            .Bind(result => GetFromDb(result.Id))
            .Bind(result => Update(result, flag))
            .Map(result => result.ToDto());
    }

    private async Task<Result<FlagDto>> VerifyExisting(FlagDto flag)
    {
        var exists = await _dbContext.Flags.AnyAsync(f =>
            f.Name.ToLower() == flag.Name.ToLower() &&
            f.Id != flag.Id);

        return exists ? Result.Failure<FlagDto>("Flag with that name already exists") : flag;
    }

    private async Task<Result<FlagEntity>> GetFromDb(int Id)
    {
        return await _dbContext.Flags
            .Where(f => f.Id == Id)
            .SingleAsync();
    }

    private async Task<Result<FlagEntity>> Update(FlagEntity flag, FlagDto dto)
    {
        flag.Value = dto.IsEnabled;
        flag.Name = dto.Name;
        await _dbContext.SaveChangesAsync();
        return flag;
    }
}