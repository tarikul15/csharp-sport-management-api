using Microsoft.EntityFrameworkCore;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Manager>()
                .Property(a => a.Email)
                .IsRequired();
            
            builder.Entity<Manager>()
                .Property(a => a.Password)
                .IsRequired();

            builder.Entity<Manager>()
                .HasKey(m => m.Id);


            builder.Entity<Team>()
                .HasKey(t => t.Id);

            builder.Entity<Team>()
                .Property(t => t.Name).IsRequired();

            builder.Entity<Team>()
                .HasIndex(t => t.Name).IsUnique();

            builder.Entity<Manager>()
                .HasOne(m=>m.Team)
                .WithOne(t => t.Manager)
                .HasForeignKey<Team>(t => t.ManagerId);

            builder.Entity<Player>()
                .HasKey(p => p.Id);

            builder.Entity<Player>()
                .Property(p => p.Id).IsRequired();
            builder.Entity<Player>()
                .Property(p => p.Name).IsRequired();
            builder.Entity<Player>()
                .Property(p => p.TeamId).IsRequired();
            builder.Entity<Player>()
                .Property(p => p.ManagerId).IsRequired();

            builder.Entity<Player>()
                .HasIndex(p => p.Name).IsUnique();

            builder.Entity<Manager>()
                .HasMany(m => m.Players)
                .WithOne(p => p.Manager)
                .HasForeignKey(p => p.ManagerId);

            builder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId);
        }
    }
}