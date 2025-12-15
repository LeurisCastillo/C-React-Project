using CandidatesChannels.Application.Services;
using CandidatesChannels.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CandidatesChannels.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWeatherAppService, WeatherAppService>();

        services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        return services;
    }
}
