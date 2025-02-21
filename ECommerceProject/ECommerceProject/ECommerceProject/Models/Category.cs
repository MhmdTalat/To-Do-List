using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models
{
    // Define an enum for departments
    public enum Department
    {
        Fruits=0,
        Vegetables=1,
        Drinks=2,
        Flavor=3,
        Meat=4,
        Chickens=5
    }

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Add a Department property to classify categories
        [Required]
        public Department Department { get; set; }

        public List<Product> Products { get; set; }
    }
}