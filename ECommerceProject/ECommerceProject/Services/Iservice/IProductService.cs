using ECommerceProject.Models;
using ECommerceProject.ViewModels;

namespace ECommerceProject.Service.Iservice
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> AddProductAsync(ProductsviewModel viewModel);
        Task<bool> UpdateProductAsync(int id, ProductsviewModel viewModel);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> IsProductInAnyCartAsync(int productId);
    }
}
