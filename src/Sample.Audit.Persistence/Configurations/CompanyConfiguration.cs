using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Audit.Persistence.Entities;

namespace Sample.Audit.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(256);
    }
}