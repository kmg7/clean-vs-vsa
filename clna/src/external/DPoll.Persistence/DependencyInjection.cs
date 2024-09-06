using DPoll.Application.Features.Presentations;
using DPoll.Application.Features.Slides;
using DPoll.Application.Features.Users;
using DPoll.Persistence.Databases.UserPresentations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DPoll.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<UserPresentationsDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("Default")),
           ServiceLifetime.Singleton);

        _ = services.AddSingleton<EFUserPresentationsRepository>();

        _ = services.AddSingleton<IUsersRepository>(p =>
            p.GetRequiredService<EFUserPresentationsRepository>());

        _ = services.AddSingleton<IPresentationsRepository>(x =>
            x.GetRequiredService<EFUserPresentationsRepository>());

        _ = services.AddSingleton<ISlidesRepository>(x =>
            x.GetRequiredService<EFUserPresentationsRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
