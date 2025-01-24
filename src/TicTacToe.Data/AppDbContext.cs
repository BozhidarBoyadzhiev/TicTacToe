using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data;
public class AppDbContext : DbContext
{
    public DbSet<ComputerMode> ComputerModes { get; set; }
    //public DbSet<MultiplayerMode> MultiplayerLeaderboard { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComputerMode>(entity =>
        {
            entity.ToTable("ComputerModes");
            entity.HasKey(c => c.Id);
                
            entity.Property(c => c.PlayerName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(c => c.PlayerColor)
                .IsRequired()
                .HasMaxLength(7); // #RRGGBB format
        });
    }
}
