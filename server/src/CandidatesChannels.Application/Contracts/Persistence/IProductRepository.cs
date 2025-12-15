using CandidatesChannels.Domain.Entities;

namespace CandidatesChannels.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? category,
        string? search,
        CancellationToken ct);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task DeleteAsync(Product product, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
