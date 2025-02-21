using ECommerceProject.Models;
using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Models.Membership;

namespace ECommerceProject.ViewModels
{
    public class RoleViewModel
    {
        [Required] 
        public string RoleName { get; set; }
        //public List<User> users { get; set; }
    }
}