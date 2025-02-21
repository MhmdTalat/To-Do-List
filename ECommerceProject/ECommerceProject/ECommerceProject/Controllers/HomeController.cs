using ECommerceProject.Data;
using ECommerceProject.Models;
 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ECommerceProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly ProductService _productService;

        // Single constructor accepting both dependencies
        public HomeController(ApplicationDbContext context/*, ProductService productService*/)
        {
            _context = context;
            //_productService = productService;
        }

        // GET: Product/Index
        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect unauthenticated users to the login page
                return RedirectToAction("Login", "Account");
            }

            // Fetch and display products for authenticated users
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
