using CandidatesChannels.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandidatesChannels.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Stock).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.Name);
    }
}
