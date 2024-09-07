using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using DPoll.Application.Features.Users;
using DPoll.Persistence.Databases.UserPresentations;
using DPoll.Persistence.Databases.UserPresentations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DPoll.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<DPollDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("Default")),
           ServiceLifetime.Singleton);

        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DPollDbContext>();
        _ = context.Database.EnsureDeleted();
        _ = context.Database.EnsureCreated();
        _ = context.AddData();

        _ = services.AddSingleton<EFUsersRepository>();
        _ = services.AddSingleton<EFPresentationsRepository>();
        _ = services.AddSingleton<EFSlidesRepository>();

        _ = services.AddSingleton<IUsersRepository>(p =>
            p.GetRequiredService<EFUsersRepository>());

        _ = services.AddSingleton<IPresentationsRepository>(x =>
            x.GetRequiredService<EFPresentationsRepository>());

        _ = services.AddSingleton<ISlidesRepository>(x =>
            x.GetRequiredService<EFSlidesRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
