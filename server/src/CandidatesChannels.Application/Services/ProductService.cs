using CandidatesChannels.Application.Common.Pagination;
using CandidatesChannels.Application.Contracts.Persistence;
using CandidatesChannels.Application.DTOs.Products;
using CandidatesChannels.Domain.Entities;

namespace CandidatesChannels.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, string? category, string? search, CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var (items, total) = await _repo.GetPagedAsync(page, pageSize, category, search, ct);
        var dtos = items.Select(ToDto).ToList();
        return new PagedResult<ProductDto>(dtos, page, pageSize, total);
    }

    public async Task<ProductDto?> GetProductAsync(Guid id, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(id, ct);
        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken ct)
    {
        var product = new Product(request.Name, request.Category, request.Price, request.Stock, request.Description);
        await _repo.AddAsync(product, ct);
        await _repo.SaveChangesAsync(ct);
        return ToDto(product);
    }

    public async Task<bool> DeleteProductAsync(Guid id, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(id, ct);
        if (product is null) return false;

        await _repo.DeleteAsync(product, ct);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Category, p.Price, p.Stock, p.Description, p.CreatedAtUtc);
}
