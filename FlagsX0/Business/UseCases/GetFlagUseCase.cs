using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Business.UseCases;

public class GetFlagsUseCase(ApplicationDbContext dbContext, IFlagUserDetails userDetails)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IFlagUserDetails _userDetails = userDetails;

    public async Task<Result<List<FlagDto>>> Execute()
    {
        var response =
            await _dbContext.Flags
                .Where(flag => flag.UserId == _userDetails.UserId)
                .AsNoTracking()
                .ToListAsync();

        return response.Select(flag => new FlagDto(flag.Id, flag.Name, flag.Value)).ToList();
    }
}