using CandidatesChannels.Application.DTOs.Weather;

namespace CandidatesChannels.Application.Contracts.External;

public interface IWeatherClient
{
    Task<WeatherResponse> GetWeatherAsync(string city, CancellationToken ct);
}
