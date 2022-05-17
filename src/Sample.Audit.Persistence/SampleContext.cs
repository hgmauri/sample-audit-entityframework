using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sample.Audit.Infrastructure.Enums;
using Sample.Audit.Persistence.Entities;

namespace Sample.Audit.Persistence;

public class SampleContext : IdentityDbContext<User, Role, Guid>
{
    public IHttpContextAccessor HttpContext { get; }
    public SampleContext(DbContextOptions options, IHttpContextAccessor httpContext)
        : base(options)
    {
        HttpContext = httpContext;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(SampleContext).Assembly);
    }

    public DbSet<Entities.Audit> Audits { get; set; }
    public DbSet<Company> Companies { get; set; }
    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }

    public override int SaveChanges()
    {
        BeforeSaveChanges().ConfigureAwait(false).GetAwaiter().GetResult();
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await BeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private async Task BeforeSaveChanges()
    {
        try
        {
            var login = HttpContext?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Base auditable)
                {
                    auditable.UpdateDate(entry.State);
                }

                if (entry.Entity is Entities.Audit || entry.State is EntityState.Detached or EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry) { TableName = entry.Entity.GetType().Name, UserId = login };

                foreach (var property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                    await Audits.AddAsync(auditEntry.ToAudit());
                }
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error saving audit");
        }
    }
}