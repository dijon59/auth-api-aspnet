using BackendApi.models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
