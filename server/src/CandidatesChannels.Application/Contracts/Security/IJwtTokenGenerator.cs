namespace CandidatesChannels.Application.Contracts.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string role);
}
