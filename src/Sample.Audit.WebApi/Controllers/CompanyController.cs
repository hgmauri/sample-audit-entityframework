using Microsoft.AspNetCore.Mvc;
using Sample.Audit.Persistence;
using Sample.Audit.Persistence.Entities;

namespace Sample.Audit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly SampleContext _context;

    public CompanyController(SampleContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<OkObjectResult> PostCompany(string name)
    {
        var result = await _context.Companies.AddAsync(new Company {Name = name });
        await _context.SaveChangesAsync();

        return Ok(result.Entity.Id.ToString());
    }

    [HttpPut]
    public async Task<OkResult> PutCompany(Guid id, string newName)
    {
        var result = await _context.Companies.FindAsync(id);

        if (result != null)
        {
            result.Name = newName;
            await _context.SaveChangesAsync();
        }
        return Ok();
    }
}