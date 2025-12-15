using CandidatesChannels.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CandidatesChannels.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).HasMaxLength(254).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
