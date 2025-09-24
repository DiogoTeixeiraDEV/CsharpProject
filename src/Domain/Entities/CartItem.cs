using System;

namespace Domain.Entities;

public class CartItem
{
    public Guid Id { get; private set; }
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; private set; }

    public Cart Cart { get; private set; } = null!;
    public Product Product { get; private set; } = null!;


    private CartItem() { }

    public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        CartId = cartId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) return;
        Quantity += amount;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        Quantity = quantity;
    }
}