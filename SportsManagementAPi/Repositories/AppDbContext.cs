using Microsoft.EntityFrameworkCore;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Result> Results { get; set; }

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

            builder.Entity<Schedule>()
                .HasKey(s => s.GameId);

            builder.Entity<Schedule>()
                .Property(s => s.HomeTeamId).IsRequired();
            builder.Entity<Schedule>()
                .Property(s => s.AwayTeamId).IsRequired();
            builder.Entity<Schedule>()
                .Property(s => s.HomeTeamName).IsRequired();
            builder.Entity<Schedule>()
                .Property(s => s.AwayTeamName).IsRequired();
            builder.Entity<Schedule>()
                .Property(s => s.ScheduledTime).IsRequired();
            builder.Entity<Schedule>()
                .Property(s => s.ManagerId).IsRequired();

            builder.Entity<Manager>()
                .HasMany(m => m.Schedules)
                .WithOne(s => s.Manager)
                .HasForeignKey(s => s.ManagerId);

            builder.Entity<Team>()
                .HasMany(t => t.Schedules)
                .WithMany(s => s.Teams);



            builder.Entity<Result>()
                .HasKey(r => r.GameId);
            
            builder.Entity<Result>()
                .Property(r => r.LoserTeamId).IsRequired();
            builder.Entity<Result>()
                .Property(r => r.WinnerTeamId).IsRequired();
            builder.Entity<Result>()
                .Property(r => r.ManagerId).IsRequired();

            builder.Entity<Manager>()
                .HasMany(m => m.Results)
                .WithMany(r => r.Managers);

            builder.Entity<Team>()
                .HasMany(t => t.Results)
                .WithMany(r => r.Teams);
                
            builder.Entity<Schedule>()
                .HasOne(s => s.Result)
                .WithOne(r => r.Schedule)
                .HasForeignKey<Result>(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}