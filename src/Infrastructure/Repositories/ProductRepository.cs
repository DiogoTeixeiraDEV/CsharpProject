using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Set<Product>().AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Set<Product>().Update(product);
        await _context.SaveChangesAsync();
    }
    
}