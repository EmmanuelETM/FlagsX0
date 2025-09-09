using FlagsX0.Data;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Business.UseCases;

public interface IAddFlagUseCase
{
    Task<bool> Execute(string flagName, string userId);
}

public class AddFlagUseCase(ApplicationDbContext dbContext) : IAddFlagUseCase
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> Execute(string flagName, string userId)
    {
        var flagAlreadyExists = await _dbContext.Flags.AnyAsync(
            flag => flag.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase) && flag.User.Id == userId
           );

        return flagAlreadyExists;
    }

    public void Example()
    {
        
    }
    
}