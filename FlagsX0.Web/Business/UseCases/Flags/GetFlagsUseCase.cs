using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class GetFlagsUseCase(ApplicationDbContext aplicationDbContext, IFlagUserDetails userDetails)
    {
        public async Task<Result<List<FlagDTO>>> Execute()
        {

            var response = await aplicationDbContext
                .Flags
                .Where(x => x.UserId == userDetails.UserId)
                .AsNoTracking()
                .ToListAsync();

            if (response == null || response.Count == 0)
            {
                return Result.Failure<List<FlagDTO>>("No flags found");
            }

            return response.Select(a => new FlagDTO(a.Name, a.Value, a.Id)).ToList();
        }
    }
}
