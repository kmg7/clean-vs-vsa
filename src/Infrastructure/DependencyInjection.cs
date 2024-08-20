namespace Infrastructure;

using Application.Presentations;
using Application.Users;
using Infrastructure.Databases.UserPresentations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<UserPresentationsDbContext>(options =>
           options.UseNpgsql(configuration.GetConnectionString("Default")),
           ServiceLifetime.Singleton);
        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton<EFUserPresentationsRepository>();

        _ = services.AddSingleton<IUsersRepository>(p =>
            p.GetRequiredService<EFUserPresentationsRepository>());

        _ = services.AddSingleton<IPresentationsRepository>(x =>
            x.GetRequiredService<EFUserPresentationsRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
