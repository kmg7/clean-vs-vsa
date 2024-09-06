using Dpoll.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DPoll.Persistence.Databases.UserPresentations;

internal class UserPresentationsDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Presentation> Presentations => Set<Presentation>();
    public DbSet<Slide> Slides => Set<Slide>();

    public UserPresentationsDbContext(DbContextOptions<UserPresentationsDbContext> options) : base(options) { }
    public UserPresentationsDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserPresentationsDbContext>
{
    public UserPresentationsDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("Default");
        var builder = new DbContextOptionsBuilder<UserPresentationsDbContext>();
        if (string.IsNullOrEmpty(connectionString))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Default");
        }
        _ = builder.UseNpgsql(connectionString);
        return new UserPresentationsDbContext(builder.Options);
    }
}
