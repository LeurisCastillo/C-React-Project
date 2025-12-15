namespace CandidatesChannels.Application.DTOs.Products;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Category,
    decimal Price,
    int Stock,
    string? Description,
    DateTime CreatedAtUtc
);
