using Microsoft.EntityFrameworkCore;
using SportsManagementAPi.Domain.Models;

namespace SportsManagementAPi.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }

        public DbSet<Team> Teams { get; set; }

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
        }
    }
}