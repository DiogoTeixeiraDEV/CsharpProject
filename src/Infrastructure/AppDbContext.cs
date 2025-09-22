using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure;


namespace Infrastructure;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<User> Users => Set<User>();
}