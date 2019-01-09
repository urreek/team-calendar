using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using team_calendar.Models;

namespace team_calendar.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Team> Teams { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");

        modelBuilder.Entity<UserTeam>().HasKey(t => new { t.UserId, t.TeamId });

        modelBuilder.Entity<UserTeam>()
            .HasOne(pt => pt.User)
            .WithMany(p => p.UserTeams)
            .HasForeignKey(pt => pt.UserId);

        modelBuilder.Entity<UserTeam>()
            .HasOne(pt => pt.Team)
            .WithMany(t => t.UserTeams)
            .HasForeignKey(pt => pt.TeamId);
    }
    }
}