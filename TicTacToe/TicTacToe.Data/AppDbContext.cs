using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data;
public class AppDbContext : DbContext
{
    public DbSet<ComputerLeaderboard> ComputerLeaderboard { get; set; }
    public DbSet<MultiplayerLeaderboard> MultiplayerLeaderboard { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ComputerLeaderboard>()
            .HasKey(c => c.Id);
        
        modelBuilder.Entity<MultiplayerLeaderboard>()
            .HasKey(m => m.Id);
        
        modelBuilder.Entity<ComputerLeaderboard>()
            .Property(c => c.PlayerName)
            .IsRequired();

        modelBuilder.Entity<MultiplayerLeaderboard>()
            .Property(m => m.Player1Name)
            .IsRequired();

        modelBuilder.Entity<MultiplayerLeaderboard>()
            .Property(m => m.Player2Name)
            .IsRequired();
    }
}
