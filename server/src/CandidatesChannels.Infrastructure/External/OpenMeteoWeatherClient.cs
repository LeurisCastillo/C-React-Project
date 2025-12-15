using System.Net.Http.Json;
using CandidatesChannels.Application.Contracts.External;
using CandidatesChannels.Application.DTOs.Weather;

namespace CandidatesChannels.Infrastructure.External;

public sealed class OpenMeteoWeatherClient : IWeatherClient
{
    private readonly HttpClient _http;

    private static readonly Dictionary<string, (double lat, double lon)> CityMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["santo domingo"] = (18.4861, -69.9312),
            ["santiago"] = (19.4792, -70.6931),
            ["punta cana"] = (18.5820, -68.4055),
            ["new york"] = (40.7128, -74.0060),
            ["madrid"] = (40.4168, -3.7038),
        };

    public OpenMeteoWeatherClient(HttpClient http) => _http = http;

    public async Task<WeatherResponse> GetWeatherAsync(string city, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(city)) city = "Santo Domingo";

        if (!CityMap.TryGetValue(city.Trim(), out var coords))
            coords = CityMap["santo domingo"];

        // Open-Meteo: no API key required
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={coords.lat}&longitude={coords.lon}&current=temperature_2m,wind_speed_10m";
        var resp = await _http.GetFromJsonAsync<OpenMeteoResponse>(url, ct)
                   ?? throw new InvalidOperationException("Weather provider returned no data.");

        return new WeatherResponse(
            City: city.Trim(),
            Latitude: coords.lat,
            Longitude: coords.lon,
            TemperatureC: resp.current.temperature_2m,
            WindSpeedKmh: resp.current.wind_speed_10m,
            TimeIso: resp.current.time
        );
    }

    private sealed record OpenMeteoResponse(Current current);
    private sealed record Current(string time, double temperature_2m, double wind_speed_10m);
}
