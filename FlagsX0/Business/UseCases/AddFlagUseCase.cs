
using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases;


public class AddFlagUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<Result<bool>> Execute(string flagName, bool isEnabled)
        => await ValidateFlag(flagName)
            .Bind(result => AddFlagToDb(result, isEnabled));

    private async Task<Result<string>> ValidateFlag(string flagName)
    {
        var flagExists = await _dbContext.Flags.Where(
            flag => flag.UserId == _userDetails.UserId && 
                    EF.Functions.Like(flag.Name.ToLower(), flagName.ToLower())
        ).AnyAsync();

        return flagExists ? 
            Result.Failure<string>("This flag already exists") 
            : flagName;
    }

    private async Task<Result<bool>> AddFlagToDb(string flagName, bool isEnabled)
    {
        FlagEntity entity = new()
        {
            Name = flagName,
            UserId = _userDetails.UserId,
            Value = isEnabled,
        };
        
        _ = await _dbContext.Flags.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}