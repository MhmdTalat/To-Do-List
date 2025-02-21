using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class ProductsviewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } // Optional, in case you store URLs

        [NotMapped] // Prevent Entity Framework from mapping this property
        public IFormFile ImageFile { get; set; }



        [Required(ErrorMessage = "Category is required.")]
        [Range(1, 6, ErrorMessage = "Category must be between 1 and 6.")]
        public int CategoryId { get; set; }

        // Other properties like Name, Description, Price, etc.


        public List<Category> Categories { get; set; }
    }
}
