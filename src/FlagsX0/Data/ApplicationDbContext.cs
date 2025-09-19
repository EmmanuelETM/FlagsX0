using FlagsX0.Business.Services;
using FlagsX0.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IFlagUserDetails userDetails
)
    : IdentityDbContext(options)
{
    public DbSet<FlagEntity> Flags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlagEntity>()
            .HasQueryFilter(a => !a.IsDeleted
                                 && a.UserId == userDetails.UserId);

        base.OnModelCreating(modelBuilder);
    }
}