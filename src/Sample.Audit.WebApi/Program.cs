using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Audit.Infrastructure.Extensions;
using Sample.Audit.Persistence;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddSerilog(builder.Configuration, "Sample Audit");
    Log.Information("Getting the motors running...");

    builder.Services.AddControllers();
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddDbContext<SampleContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.BuildServiceProvider().GetService<SampleContext>()?.Database.Migrate();

    builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Sample Audit", Version = "v1"}); });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Sample Audit v1"));
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}