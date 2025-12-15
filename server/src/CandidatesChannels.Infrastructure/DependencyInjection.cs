using CandidatesChannels.Application.Contracts.External;
using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Application.Contracts.Security;
using CandidatesChannels.Infrastructure.External;
using CandidatesChannels.Infrastructure.Persistence;
using CandidatesChannels.Infrastructure.Repositories;
using CandidatesChannels.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CandidatesChannels.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddHttpClient<IWeatherClient, OpenMeteoWeatherClient>();

        return services;
    }
}
