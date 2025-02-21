using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.Service.Iservice;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [Authorize(Roles = "Admin,Trader")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            var categories = await _productService.GetCategoriesAsync();
            return View(new ProductsviewModel { Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> SaveNew(ProductsviewModel viewModel)
        {
            if (await _productService.AddProductAsync(viewModel))
                return RedirectToAction("Index");

            var categories = await _productService.GetCategoriesAsync();
            viewModel.Categories = categories;
            return View("New", viewModel);
        }

        [Authorize(Roles = "Admin,Trader")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var viewModel = new ProductsviewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = await _productService.GetCategoriesAsync()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Trader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductsviewModel viewModel)
        {
            if (await _productService.UpdateProductAsync(id, viewModel))
                return RedirectToAction("Index");

            viewModel.Categories = await _productService.GetCategoriesAsync();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Trader")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isInCart = await _productService.IsProductInAnyCartAsync(id);
            if (isInCart)
            {
                if (User.IsInRole("Admin"))
                {
                    TempData["ErrorMessage"] = "This product is in one or more user carts and cannot be deleted.";
                }
                else if (User.IsInRole("Trader"))
                {
                    TempData["ErrorMessage"] = "Please call Support because this product is in one or more user carts.";
                }
                return RedirectToAction(nameof(Index));
            }

            if (await _productService.DeleteProductAsync(id))
            {
                TempData["SuccessMessage"] = "Product deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        // Action to list all products
        public IActionResult ListOfProduct()
        {
            var products = _context.Products
                                   .Include(p => p.Category)
                                   .ToList();

            var productViewModels = products.Select(p => new ProductsviewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                ImageFile = p.ImageFile,
                CategoryId = p.CategoryId,
            }).ToList();

            return View(productViewModels);
        }

        // Action to view product details by id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductsviewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                
            };

            return View(viewModel);
        }
    }
}
