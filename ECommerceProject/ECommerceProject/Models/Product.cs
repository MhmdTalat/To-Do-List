using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } // Optional, in case you store URLs

        [NotMapped] // Prevent Entity Framework from mapping this property
        public IFormFile ImageFile { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation property
       
        // Navigation properties for OrderItems and CartItems



    }
}
