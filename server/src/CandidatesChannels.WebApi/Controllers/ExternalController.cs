using CandidatesChannels.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesChannels.WebApi.Controllers;

[ApiController]
[Route("api/external")]
public sealed class ExternalController : ControllerBase
{
    private readonly IWeatherAppService _weather;

    public ExternalController(IWeatherAppService weather) => _weather = weather;

    // GET /api/external/weather?city=Santo%20Domingo
    [HttpGet("weather")]
    public async Task<IActionResult> Weather([FromQuery] string city = "Santo Domingo", CancellationToken ct = default)
    {
        var result = await _weather.GetWeatherAsync(city, ct);
        return Ok(result);
    }
}
