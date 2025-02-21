using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProject.Services
{
    public class TraderService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TraderService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddNewTraderAsync(RegisterViewModel newUserModel)
        {
            var userModel = new ApplicationUser
            {
                UserName = newUserModel.Username,
                Email = newUserModel.Email,
                Address = $"{newUserModel.Street}, {newUserModel.City}, {newUserModel.State}"
            };

            var result = await _userManager.CreateAsync(userModel, newUserModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userModel, "Trader");
            }

            return result;
        }

        public async Task<List<RegisterViewModel>> GetAllAdminsAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(admin => new RegisterViewModel
            {
                Username = admin.UserName,
                Email = admin.Email
            }).ToList();
        }
    }
}
