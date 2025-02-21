using ECommerceProject.Models;
using System.ComponentModel.DataAnnotations;



    namespace ECommerceProject.ViewModels
    {
        public class CategoryViewModel
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public Department Department { get; set; }
        }
    }



