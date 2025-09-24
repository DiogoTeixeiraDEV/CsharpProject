using System.Data;
using Domain.Entities;
using Domain.Repositories; 

namespace Application.Services;

public class CartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;


    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> GetCartByUserIdAsync(Guid userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
            return new CartDto { Items = new List<CartItemDto>(), UserId = userId };

        return MapToDto(cart);
    }

    public async Task<CartDto> AddItemAsync(Guid userId, Guid productId, int quantity)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId) ?? new Cart(userId);

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            cart.Items.Add(new CartItem(cart.Id, product.Id, quantity, unitPrice: product.Price));
        }
        await _cartRepository.UpdateAsync(cart);
        return MapToDto(cart);
    }

    public async Task<CartDto> RemoveItemAsync(Guid userId, Guid productId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
            throw new KeyNotFoundException("Carrinho não encontrado.");

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            throw new KeyNotFoundException("Item do carrinho não encontrado.");

        cart.Items.Remove(item);
        await _cartRepository.UpdateAsync(cart);
        return MapToDto(cart);
    }
        

    private CartDto MapToDto(Cart cart)
    {
        return new CartDto
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(item => new CartItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}