using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        await _cartService.AddToCart(productId, quantity, user.Id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _cartService.GetCartItems(user.Id);
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        await _cartService.RemoveFromCart(cartItemId, user.Id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetCartTotal()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Json(0);
        }

        var total = await _cartService.GetCartTotal(user.Id);
        return Json(total);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        await _cartService.UpdateQuantity(cartItemId, quantity, user.Id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(string paymentMethod)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        try
        {
            await _cartService.Checkout(user.Id, paymentMethod);
            TempData["Success"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderConfirmation");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }
}
