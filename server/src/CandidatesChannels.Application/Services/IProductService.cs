using CandidatesChannels.Application.Common.Pagination;
using CandidatesChannels.Application.DTOs.Products;

namespace CandidatesChannels.Application.Services;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, string? category, string? search, CancellationToken ct);
    Task<ProductDto?> GetProductAsync(Guid id, CancellationToken ct);
    Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken ct);
    Task<bool> DeleteProductAsync(Guid id, CancellationToken ct);
}
