using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Checkout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch cart items for the logged-in user
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == user.Id)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        // Calculate total amount and discount
        var totalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);
        var discount = cartItems
            .Where(item => item.Product.Price > 10)
            .Sum(item => item.Quantity * item.Product.Price * 0.10m); // 10% discount for eligible products

        var finalAmount = totalAmount - discount;

        // Pass calculated values to the view
        ViewBag.TotalAmount = totalAmount;
        ViewBag.Discount = discount;
        ViewBag.FinalAmount = finalAmount;

        var domain = $"{Request.Scheme}://{Request.Host}";

        // Stripe session options
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = cartItems.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name,
                    },
                    UnitAmount = (long)((item.Product.Price > 10
                        ? item.Product.Price * 0.90m // Apply discount
                        : item.Product.Price) * 100), // Convert to cents
                },
                Quantity = item.Quantity,
            }).ToList(),
            Mode = "payment",
            SuccessUrl = $"{domain}/Payment/Success",
            CancelUrl = $"{domain}/Payment/Cancel",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        // Redirect to Stripe payment page
        return Redirect(session.Url);
    }

    public async Task<IActionResult> Success()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Fetch user cart items
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == user.Id)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (cartItems.Any())
        {
            // Create an order from cart items
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price),
                OrderItems = cartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                }).ToList(),
            };

            // Save order to database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the cart
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        return View("Success"); // Success view should be created
    }

    public IActionResult Cancel()
    {
        TempData["Error"] = "Payment was canceled.";
        return RedirectToAction("Index", "Cart");
    }
}
