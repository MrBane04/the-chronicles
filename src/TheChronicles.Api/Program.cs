using Microsoft.EntityFrameworkCore;
using TheChronicles.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found. Set the ConnectionStrings__DefaultConnection environment variable.");

builder.Services.AddDbContext<TheChroniclesDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapGet("/health/database", async (
    TheChroniclesDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    try
    {
        var result = await dbContext.Database
            .SqlQueryRaw<int>("SELECT 1 AS \"Value\"")
            .SingleAsync(cancellationToken);

        return result == 1
            ? Results.Ok(new { status = "Healthy" })
            : Results.Json(
                new { status = "Unhealthy" },
                statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch (Exception)
    {
        return Results.Json(
            new { status = "Unhealthy" },
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }
});

app.Run();
