using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Application.Contracts.Security;
using CandidatesChannels.Application.DTOs.Auth;

namespace CandidatesChannels.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public AuthService(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.GetByEmailAsync(email, ct);
        if (user is null) return null;

        if (!_hasher.Verify(request.Password, user.PasswordHash)) return null;

        var token = _jwt.GenerateToken(user.Email, user.Role);
        return new LoginResponse(token, user.Email, user.Role);
    }
}
