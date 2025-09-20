using FlagsX0.Data;
using FlagsX0.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases.Flags;

public class DeleteFlagUseCase(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Result<bool>> Execute(string flagName)
    {
        return await GetFromDb(flagName).Bind(DeleteFlag);
    }

    private async Task<Result<FlagEntity>> GetFromDb(string flagName)
    {
        var result = await _dbContext.Flags
            .Where(f =>
                f.Name.ToLower() == flagName.ToLower())
            .SingleOrDefaultAsync();

        return result ?? Result.Failure<FlagEntity>("Flag not found");
    }

    private async Task<Result<bool>> DeleteFlag(FlagEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedTimeUtc = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}