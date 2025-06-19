using IntapTest.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IntapTest.Data
{
    public class IntapTestDbContext : IdentityDbContext<User,
        Role,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public IntapTestDbContext(DbContextOptions<IntapTestDbContext> options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<TimeActivity> TimeActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users")
                            .HasIndex(u => u.Email)
                            .IsUnique();
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Activity>().ToTable("Activities");
            builder.Entity<TimeActivity>().ToTable("TimeActivities");
        }
    }
}
