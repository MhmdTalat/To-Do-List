using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.ViewModels
{
    public class FeedbackViewModel
    {
        
        
       public int FeedbackId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Comment must not exceed 500 characters.")]
        public string Comment { get; set; }

        [Required]
        [Phone]
        
        [StringLength(15, MinimumLength = 11, ErrorMessage = "Phone number must be at least 11 characters long.")]
        [RegularExpression(@"^0\d{10,14}$", ErrorMessage = "Phone number must start with '0' and be at least 11 digits long.")]
        public string PhoneNumber { get; set; }
        public DateTime? DateSubmitted { get; set; }
    }

}
