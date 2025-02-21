using Microsoft.AspNetCore.Identity;

namespace ECommerceProject.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        // Foreign key to ApplicationUser
        public string UserId { get; set; }  // Foreign key to the ApplicationUser table
        public ApplicationUser User { get; set; }  // Navigation property to ApplicationUser
    }
}
