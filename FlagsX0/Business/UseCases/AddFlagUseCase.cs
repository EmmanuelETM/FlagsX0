using System.Security.Claims;
using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Business.UseCases;


public class AddFlagUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<bool> Execute(string flagName, bool isEnabled)
    {
        var userId = _userDetails.UserId;

        FlagEntity entity = new()
        {
            Name = flagName,
            UserId = userId,
            Value = isEnabled
        };

        var response = await _dbContext.Flags.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}