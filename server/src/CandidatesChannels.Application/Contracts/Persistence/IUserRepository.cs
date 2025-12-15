using CandidatesChannels.Domain.Entities;

namespace CandidatesChannels.Application.Contracts.Persistence;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
