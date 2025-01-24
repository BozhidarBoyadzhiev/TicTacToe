using Microsoft.EntityFrameworkCore;

namespace TicTacToe.Data;
public class AppDbContext : DbContext
{
    public DbSet<ComputerMode> ComputerModes { get; set; }
    public DbSet<MultiplayerMode> MultiplayerModes { get; set; }

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
        
        modelBuilder.Entity<MultiplayerMode>(entity =>
        {
            entity.ToTable("MultiplayerModes");
            entity.HasKey(m => m.Id);
        
            entity.Property(m => m.Player1Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(m => m.Player2Name)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(m => m.Player1Color)
                .IsRequired()
                .HasMaxLength(7); // #RRGGBB format
            
            entity.Property(m => m.Player2Color)
                .IsRequired()
                .HasMaxLength(7); // #RRGGBB format
            
            entity.Property(m => m.Result)
                .IsRequired()
                .HasMaxLength(20); // "Player1 Win", "Player2 Win", "Draw"

            entity.HasIndex(m => m.Date);
        });

    }
}
