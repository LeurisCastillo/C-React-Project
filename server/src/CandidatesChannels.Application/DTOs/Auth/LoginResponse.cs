namespace CandidatesChannels.Application.DTOs.Auth;

public sealed record LoginResponse(string AccessToken, string Email, string Role);
