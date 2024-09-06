using Api.Features.Presentations.Entities;
using Api.Features.Slides.Entities;
using Api.Features.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Api.Common.Persistence;

public class BaseDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Slide> Slides { get; set; }

    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }
    public BaseDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
{
    public BaseDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("Default");
        var builder = new DbContextOptionsBuilder<BaseDbContext>();
        if (string.IsNullOrEmpty(connectionString))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Default");
        }
        builder.UseNpgsql(connectionString);
        return new BaseDbContext(builder.Options);
    }
}
