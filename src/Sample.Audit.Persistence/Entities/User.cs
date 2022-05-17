using Microsoft.AspNetCore.Identity;

namespace Sample.Audit.Persistence.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}