namespace CandidatesChannels.Application.DTOs.Weather;

public sealed record WeatherResponse(
    string City,
    double Latitude,
    double Longitude,
    double TemperatureC,
    double WindSpeedKmh,
    string? TimeIso
);
