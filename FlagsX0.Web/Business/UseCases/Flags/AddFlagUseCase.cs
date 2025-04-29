using FlagsX0.Web.Business.UserInfo;
using FlagsX0.Web.Data;
using FlagsX0.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FlagsX0.Web.Business.UseCases.Flags
{
    public class AddFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails flagUserDetails)
    {
        public async Task<Result<bool>> Execute(string flagName, bool isActive)
        => await ValidateFlag(flagName)
                .Bind(x => AddFlagToDB(x, isActive));

        private async Task<Result<String>> ValidateFlag(string flagName)
        {
            bool flagExists = await applicationDbContext.Flags.Where(a => a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase)).AnyAsync();

            if (flagExists)
            {
                return Result.Failure<string>($"Flag {flagName} already exists");
            }

            return flagName;
        }

        private async Task<Result<bool>> AddFlagToDB(string flagName, bool isActive)
        {
            FlagEntity entity = new()
            {
                Name = flagName,
                UserId = flagUserDetails.UserId,
                Value = isActive,
            };

            _ = await applicationDbContext.Flags.AddAsync(entity);
            await applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
