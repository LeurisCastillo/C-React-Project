using CandidatesChannels.Application.DTOs.Auth;

namespace CandidatesChannels.Application.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
}
