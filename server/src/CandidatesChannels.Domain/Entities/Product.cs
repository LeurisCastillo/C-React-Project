using CandidatesChannels.Domain.Common;

namespace CandidatesChannels.Domain.Entities;

public sealed class Product : BaseEntity
{
    private Product() { } // EF Core

    public Product(string name, string category, decimal price, int stock, string? description)
    {
        SetName(name);
        SetCategory(category);
        SetPrice(price);
        SetStock(stock);
        Description = description;
    }

    public string Name { get; private set; } = default!;
    public string Category { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string? Description { get; private set; }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required.");
        if (name.Length > 200) throw new DomainException("Name must be <= 200 characters.");
        Name = name.Trim();
    }

    public void SetCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category)) throw new DomainException("Category is required.");
        if (category.Length > 100) throw new DomainException("Category must be <= 100 characters.");
        Category = category.Trim();
    }

    public void SetPrice(decimal price)
    {
        if (price < 0) throw new DomainException("Price must be >= 0.");
        Price = price;
    }

    public void SetStock(int stock)
    {
        if (stock < 0) throw new DomainException("Stock must be >= 0.");
        Stock = stock;
    }
}

public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
