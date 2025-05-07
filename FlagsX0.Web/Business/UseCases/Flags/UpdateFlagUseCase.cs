using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.Data.Entities;
using FlagsX0.Web.DTOs;
using FlagX0.Web.Business.Mappers;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class UpdateFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
    {
        public async Task<Result<FlagDTO>> Execute(FlagDTO flagDto)
            => await VerifyUniqueName(flagDto)
                     .Bind(x => GetFromDb(x.Id))
                     .Bind(x => Update(x, flagDto))
                     .Map(x => x.ToDto());

        private async Task<Result<FlagDTO>> VerifyUniqueName(FlagDTO flagDto)
        {
            bool exists =
                await applicationDbContext.Flags
                      .AnyAsync(a => a.UserId == userDetails.UserId
                      && a.Name.Equals(flagDto.Name, StringComparison.CurrentCultureIgnoreCase));

            if (exists)
            {
                return Result.Failure<FlagDTO>("Flag with the same name exists!");
            }

            return flagDto;
        }

        private async Task<Result<FlagEntity>> GetFromDb(int id)
            => await applicationDbContext.Flags
                     .Where(a => a.UserId == userDetails.UserId && a.Id == id)
                     .SingleAsync();

        private async Task<Result<FlagEntity>> Update(FlagEntity entity, FlagDTO flagDto)
        {
            entity.Value = flagDto.IsEnabled;
            entity.Name = flagDto.Name;
            await applicationDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
