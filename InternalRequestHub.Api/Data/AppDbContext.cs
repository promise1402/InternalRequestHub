using InternalRequestHub.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalRequestHub.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Request> Requests => Set<Request>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
