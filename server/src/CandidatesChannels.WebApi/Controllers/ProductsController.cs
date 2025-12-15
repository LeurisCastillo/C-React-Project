using CandidatesChannels.Application.DTOs.Products;
using CandidatesChannels.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesChannels.WebApi.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _products;
    private readonly IValidator<CreateProductRequest> _createValidator;

    public ProductsController(IProductService products, IValidator<CreateProductRequest> createValidator)
    {
        _products = products;
        _createValidator = createValidator;
    }

    // GET /api/products?page=1&pageSize=10&category=Electronics&search=mouse
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? category = null, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await _products.GetProductsAsync(page, pageSize, category, search, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _products.GetProductAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return ValidationProblem(new ValidationProblemDetails(errors));
        }

        var created = await _products.CreateProductAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _products.DeleteProductAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}
