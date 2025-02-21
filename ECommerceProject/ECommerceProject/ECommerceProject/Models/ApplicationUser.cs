using Microsoft.AspNetCore.Identity;

namespace ECommerceProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Comment { get; set; } // Nullable Comment
        public string Address { get; set; } = "Default Address";
        public DateTime? DateSubmitted { get; set; }

        // Assuming ApplicationUser extends IdentityUser
      
    }

}
