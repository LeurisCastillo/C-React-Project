using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Domain.Entities;
using CandidatesChannels.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CandidatesChannels.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email.Trim().ToLowerInvariant(), ct);

    public async Task AddAsync(User user, CancellationToken ct) =>
        await _db.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
