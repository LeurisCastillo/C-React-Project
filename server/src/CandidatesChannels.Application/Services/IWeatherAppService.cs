using CandidatesChannels.Application.DTOs.Weather;

namespace CandidatesChannels.Application.Services;

public interface IWeatherAppService
{
    Task<WeatherResponse> GetWeatherAsync(string city, CancellationToken ct);
}
