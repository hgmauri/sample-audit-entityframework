using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sample.Audit.Persistence.Entities;

public class Base
{
    [Key] 
    [Required]
    public virtual Guid Id { get; set; } = Guid.NewGuid();

    public virtual DateTime? CreatedAt { get; set; }

    public virtual DateTime? UpdatedAt { get; set; }

    public void UpdateDate(EntityState state)
    {
        switch (state)
        {
            case EntityState.Added:
            case EntityState.Detached:
                CreatedAt = DateTime.Now;

                break;
            case EntityState.Modified:
                UpdatedAt = DateTime.Now;

                break;
        }
    }
}