using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class GetFlagsUseCase(ApplicationDbContext aplicationDbContext, IFlagUserDetails userDetails)
    {
        public async Task<List<FlagDTO>> Execute()
        {

            var response = await aplicationDbContext
                .Flags
                .Where(x => x.UserId == userDetails.UserId)
                .AsNoTracking()
                .ToListAsync();

            return response.Select(a => new FlagDTO(a.Name, a.Value)).ToList();
        }
    }
}
