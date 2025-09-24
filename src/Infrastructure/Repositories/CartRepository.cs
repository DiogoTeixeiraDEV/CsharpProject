using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        await _context.Set<Cart>().AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task DeleteAsync(Guid id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Set<Cart>().Update(cart);
        await _context.SaveChangesAsync();
    }
}