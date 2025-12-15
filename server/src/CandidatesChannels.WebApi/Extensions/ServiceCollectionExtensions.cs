using CandidatesChannels.WebApi.Middleware;

namespace CandidatesChannels.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
        return services;
    }
}
