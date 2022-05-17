using Microsoft.AspNetCore.Identity;

namespace Sample.Audit.Persistence.Entities;

public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
}