using Microsoft.EntityFrameworkCore;
using Hydra.Core.Models;

namespace Hydra.Data.Database;

public class HydraDbContext : DbContext
{
    public HydraDbContext(DbContextOptions<HydraDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<HydrationEntry> HydrationEntries { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;
    public DbSet<Reminder> Reminders { get; set; } = null!;
    public DbSet<VirtualPet> VirtualPets { get; set; } = null!;
    public DbSet<DailyStatistic> DailyStatistics { get; set; } = null!;
    public DbSet<DailyChallenge> DailyChallenges { get; set; } = null!;
    public DbSet<UserPreferences> UserPreferences { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(eb =>
        {
            eb.HasKey(u => u.Id);
            eb.Property(u => u.Name).IsRequired();
        });

        modelBuilder.Entity<HydrationEntry>(eb =>
        {
            eb.HasKey(h => h.Id);
            eb.Property(h => h.AmountMl).IsRequired();
        });

        modelBuilder.Entity<Reminder>(eb =>
        {
            eb.HasKey(r => r.Id);
            eb.Property(r => r.Message).IsRequired();
        });

        modelBuilder.Entity<VirtualPet>(eb =>
        {
            eb.HasKey(p => p.Id);
            eb.Property(p => p.Name).IsRequired();
        });

        modelBuilder.Entity<Achievement>(eb =>
        {
            eb.HasKey(a => a.Id);
            eb.Property(a => a.Name).IsRequired();
        });
    }
}
