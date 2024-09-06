using Api.Common.Persistence;
using Api.Features.Presentations.Persistence;
using Api.Features.Slides.Persistence;
using Api.Features.Users.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        _ = services.AddMediatR(config =>
        {
            _ = config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<BaseDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("Default")),
           ServiceLifetime.Singleton);
        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton<IUsersRepository>(p =>
            p.GetRequiredService<EfUserRepository>());

        _ = services.AddSingleton<IPresentationsRepository>(x =>
            x.GetRequiredService<EfPresentationRepository>());

        _ = services.AddSingleton<ISlidesRepository>(x =>
            x.GetRequiredService<EfSlideRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
