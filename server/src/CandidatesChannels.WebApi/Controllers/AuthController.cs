using CandidatesChannels.Application.DTOs.Auth;
using CandidatesChannels.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesChannels.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["email"] = ["Email is required."],
                ["password"] = ["Password is required."]
            }));
        }

        var result = await _auth.LoginAsync(request, ct);
        return result is null
            ? Unauthorized(new { message = "Invalid credentials." })
            : Ok(result);
    }
}
