using FlagsX0.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlagsX0.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<FlagEntity> Flags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FlagEntity>().HasQueryFilter(a => !a.IsDeleted);
            base.OnModelCreating(builder);
        }
    }
}
