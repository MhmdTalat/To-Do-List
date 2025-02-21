using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface ICartService
{
    Task AddToCart(int productId, int quantity, string userId);
    Task<IEnumerable<CartItem>> GetCartItems(string userId);
    Task RemoveFromCart(int cartItemId, string userId);
    Task UpdateQuantity(int cartItemId, int quantity, string userId);
    Task<decimal> GetCartTotal(string userId);
    Task Checkout(string userId, string paymentMethod);
}



public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddToCart(int productId, int quantity, string userId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null) throw new Exception("Product not found");

        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                UserId = userId
            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CartItem>> GetCartItems(string userId)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();
    }

    public async Task RemoveFromCart(int cartItemId, string userId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateQuantity(int cartItemId, int quantity, string userId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

        if (cartItem != null)
        {
            if (quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            else
            {
                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetCartTotal(string userId)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .SumAsync(ci => ci.Quantity * ci.Product.Price);
    }

    public async Task Checkout(string userId, string paymentMethod)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
        {
            throw new Exception("Cart is empty");
        }

        var totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

        // Simulate payment processing
        bool paymentSuccessful = paymentMethod == "Credit Card" || paymentMethod == "PayPal";

        if (!paymentSuccessful)
        {
            throw new Exception("Payment failed");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalAmount = totalAmount,
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }
}

