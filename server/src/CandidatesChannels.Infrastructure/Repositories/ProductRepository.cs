using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Domain.Entities;
using CandidatesChannels.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CandidatesChannels.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? category,
        string? search,
        CancellationToken ct)
    {
        IQueryable<Product> query = _db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category.Trim());

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Name.Contains(s) || (p.Description != null && p.Description.Contains(s)));
        }

        query = query.OrderByDescending(p => p.CreatedAtUtc);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Product product, CancellationToken ct) =>
        await _db.Products.AddAsync(product, ct);

    public Task DeleteAsync(Product product, CancellationToken ct)
    {
        _db.Products.Remove(product);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
        _db.Products.AnyAsync(p => p.Id == id, ct);

    public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
}
