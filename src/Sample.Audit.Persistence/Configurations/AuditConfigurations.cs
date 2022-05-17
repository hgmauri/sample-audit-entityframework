using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Audit.Persistence.Configurations;

public class AuditConfigurations : IEntityTypeConfiguration<Entities.Audit>
{
    public void Configure(EntityTypeBuilder<Entities.Audit> builder)
    {
        builder.ToTable("AuditLogs");

        builder.Property(p => p.UserId).HasColumnType("varchar").HasMaxLength(100);
        builder.Property(p => p.Type).HasColumnType("varchar").HasMaxLength(100);
        builder.Property(p => p.TableName).HasColumnType("varchar").HasMaxLength(100);
        builder.Property(p => p.OldValues).HasColumnType("varchar").HasMaxLength(8000);
        builder.Property(p => p.NewValues).HasColumnType("varchar").HasMaxLength(8000);
        builder.Property(p => p.AffectedColumns).HasColumnType("varchar").HasMaxLength(8000);
        builder.Property(p => p.PrimaryKey).HasColumnType("varchar").HasMaxLength(8000);
    }
}