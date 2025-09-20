using FlagsX0.Business.Services;
using FlagsX0.Data;
using FlagsX0.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.UnitTest.Services;

public class MemoDb
{
    public MemoDb(IFlagUserDetails userDetails)
    {
        var dbOptions =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        UserId = userDetails.UserId;
        Context = new ApplicationDbContext(dbOptions, userDetails);
    }

    public ApplicationDbContext Context { get; }
    private string UserId { get; }

    public async Task<MemoDb> WithFlagsAsync()
    {
        var flags = new[]
        {
            new FlagEntity { Name = "Flag1", Value = true, UserId = UserId },
            new FlagEntity { Name = "Flag2", Value = false, UserId = UserId },
            new FlagEntity { Name = "Flag3", Value = true, UserId = UserId }
        };

        Context.Flags.AddRange(flags);
        await Context.SaveChangesAsync();
        return this;
    }

    public async Task<MemoDb> WithUserFlagsAsync(string userId, params (string name, bool value)[] flags)
    {
        var flagEntities = flags.Select(f => new FlagEntity
        {
            Name = f.name,
            Value = f.value,
            UserId = userId
        }).ToArray();

        Context.Flags.AddRange(flagEntities);
        await Context.SaveChangesAsync();
        return this;
    }
}