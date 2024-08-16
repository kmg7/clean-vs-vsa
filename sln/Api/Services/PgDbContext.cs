using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PgDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Presentation> Presentations => Set<Presentation>();
    public DbSet<PresentationContent> PresentationContents => Set<PresentationContent>();
    public DbSet<PresentationSession> PresentationSessions => Set<PresentationSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=demodb;Username=demouser;Password=demopassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PresentationContent>()
            .Property(p => p.Slides)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
