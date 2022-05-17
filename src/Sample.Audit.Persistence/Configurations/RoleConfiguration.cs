using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Audit.Persistence.Entities;

namespace Sample.Audit.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(256);
        builder.Property(p => p.NormalizedName).HasColumnType("varchar").HasMaxLength(256);
        builder.Property(p => p.ConcurrencyStamp).HasColumnType("varchar").HasMaxLength(256);
    }
}