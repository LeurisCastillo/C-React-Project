using CandidatesChannels.Domain.Common;

namespace CandidatesChannels.Domain.Entities;

public sealed class User : BaseEntity
{
    private User() { } // EF Core

    public User(string email, string passwordHash, string role)
    {
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
    }

    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string Role { get; private set; } = "User";
}
