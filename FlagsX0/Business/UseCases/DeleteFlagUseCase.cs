using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases;

public class DeleteFlagUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<Result<bool>> Execute(string flagName)
    {
        return await GetFromDb(flagName).Bind(DeleteEntity);
    }

    private async Task<Result<FlagEntity>> GetFromDb(string flagName)
    {
        return await _dbContext.Flags
            .Where(f => f.UserId == _userDetails.UserId &&
                        f.Name.ToLower() == flagName.ToLower())
            .SingleAsync();
    }

    private async Task<Result<bool>> DeleteEntity(FlagEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedTimeUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}