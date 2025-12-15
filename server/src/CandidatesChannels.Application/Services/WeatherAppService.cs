using CandidatesChannels.Application.Contracts.External;
using CandidatesChannels.Application.DTOs.Weather;

namespace CandidatesChannels.Application.Services;

public sealed class WeatherAppService : IWeatherAppService
{
    private readonly IWeatherClient _client;

    public WeatherAppService(IWeatherClient client) => _client = client;

    public Task<WeatherResponse> GetWeatherAsync(string city, CancellationToken ct) =>
        _client.GetWeatherAsync(city, ct);
}
