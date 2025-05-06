using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.Data.Entities;
using FlagsX0.Web.DTOs;
using FlagX0.Web.Business.Mappers;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class GetSingleFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
    {
        public async Task<Result<FlagDTO>> Execute(string flagName)
            => await GetFromDb(flagName).Map(x => x.ToDto());

        private async Task<Result<FlagEntity>> GetFromDb(string flagName)
            => await applicationDbContext.Flags
                    .Where(a => a.UserId == userDetails.UserId
                           && a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
                    .AsNoTracking()
                    .SingleAsync();
    }
}
