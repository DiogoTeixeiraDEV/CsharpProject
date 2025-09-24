using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Cart
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime createdAt { get; private set; } = DateTime.UtcNow;
    public DateTime updatedAt { get; private set; } = DateTime.UtcNow;
    public User User { get; private set; } = null!;
    public List<CartItem> Items { get; private set; } = new();

    private Cart() { }

    public Cart(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        createdAt = DateTime.UtcNow;
        updatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        var existingItem = Items.Find(item => item.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new CartItem(Id, productId, quantity, unitPrice));
        }
        updatedAt = DateTime.UtcNow;
    }

    public void RemoveItem(Guid productId)
    {
        var item = Items.Find(i => i.ProductId == productId);
        if (item != null)
        {
            Items.Remove(item);
        }
        updatedAt = DateTime.UtcNow;
    }

    public void Clear()
    {
        Items.Clear();
        updatedAt = DateTime.UtcNow;
    }

    public void Checkout()
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot checkout an empty cart.");
        }
        updatedAt = DateTime.UtcNow;
    }

    public decimal GetTotalAmount()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.Quantity * item.UnitPrice;
        }
        return total;
    }
}