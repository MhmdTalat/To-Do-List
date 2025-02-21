using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminCartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // View all carts for all users
    public async Task<IActionResult> Index()
    {
        var carts = await _context.CartItems
            .Include(ci => ci.Product)
            .Include(ci => ci.User)
            .ToListAsync();

        return View(carts);
    }

    // View cart items for a specific user
    public async Task<IActionResult> UserCart(string userId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        return View(cartItems);
    }

    // Delete a cart item by ID
    [HttpPost]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);

        if (cartItem == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Cart item deleted successfully!";
        return RedirectToAction("Index");
    }

    // Clear all cart items for a specific user
    [HttpPost]
    public async Task<IActionResult> ClearUserCart(string userId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
        {
            TempData["Error"] = "No items found for this user!";
            return RedirectToAction("Index");
        }

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        TempData["Success"] = "User's cart cleared successfully!";
        return RedirectToAction("Index");
    }

    // Clear all carts for all users
    [HttpPost]
    public async Task<IActionResult> ClearAllCarts()
    {
        var allCartItems = await _context.CartItems.ToListAsync();

        _context.CartItems.RemoveRange(allCartItems);
        await _context.SaveChangesAsync();

        TempData["Success"] = "All carts cleared successfully!";
        return RedirectToAction("Index");
    }

    // POST method to delete user by username
  
    // GET method to render the delete user form

}

 

 
