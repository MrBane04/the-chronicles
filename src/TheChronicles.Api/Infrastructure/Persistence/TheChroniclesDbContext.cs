using Microsoft.EntityFrameworkCore;

namespace TheChronicles.Api.Infrastructure.Persistence;

public sealed class TheChroniclesDbContext(DbContextOptions<TheChroniclesDbContext> options)
    : DbContext(options)
{
}
