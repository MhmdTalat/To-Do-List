using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.Service.Iservice;
using ECommerceProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ECommerceProject.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<bool> AddProductAsync(ProductsviewModel viewModel)
        {
            if (viewModel == null || string.IsNullOrEmpty(viewModel.Name) || viewModel.CategoryId == 0)
                return false;

            var filePath = await SaveImageAsync(viewModel.ImageFile);
            if (filePath == null) return false;

            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                CategoryId = viewModel.CategoryId,
                ImageUrl = filePath
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductsviewModel viewModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryId = viewModel.CategoryId;

            if (viewModel.ImageFile != null)
            {
                var filePath = await SaveImageAsync(viewModel.ImageFile);
                if (filePath != null)
                    product.ImageUrl = filePath;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsProductInAnyCartAsync(int productId)
        {
            return await _context.CartItems.AnyAsync(ci => ci.ProductId == productId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(fileExtension))
                return null;

            var uniqueFileName = Guid.NewGuid() + fileExtension;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{uniqueFileName}";
        }
    }
}
