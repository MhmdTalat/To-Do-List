using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotalPrice { get; set; }
        public ApplicationUser User { get; set; }  // Holds the current logged-in user
    }
}
