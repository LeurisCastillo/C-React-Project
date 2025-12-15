namespace CandidatesChannels.Application.DTOs.Products;

public sealed record CreateProductRequest(
    string Name,
    string Category,
    decimal Price,
    int Stock,
    string? Description
);
