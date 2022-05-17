using Microsoft.AspNetCore.Mvc;
using Sample.Audit.Persistence;
using Sample.Audit.Persistence.Entities;

namespace Sample.Audit.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogger<LogsController> _logger;
    private readonly SampleContext _context;

    public LogsController(ILogger<LogsController> logger, SampleContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("cidades")]
    public async Task<string> GetCitiesAsync()
    {
        await _context.Companies.AddAsync(new Company {Name = "haauhauhhua"});
        await _context.SaveChangesAsync();

        return null;
    }

    [HttpGet("thread")]
    public IActionResult GetThreadAsync()
    {
        Task.Delay(3000);

        return Ok();
    }

    [HttpGet("exception")]
    public IActionResult GetException()
    {
        throw new ArgumentException("Erro");
    }
}