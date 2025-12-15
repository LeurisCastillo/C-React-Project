namespace CandidatesChannels.Infrastructure.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = "CandidatesChannels";
    public string Audience { get; init; } = "CandidatesChannels";
    public string Key { get; init; } = "CHANGE_ME_PLEASE_32+_CHARS_LONG_SECRET_KEY";
    public int ExpirationMinutes { get; init; } = 60;
}
