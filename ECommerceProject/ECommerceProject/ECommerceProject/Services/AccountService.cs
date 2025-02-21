using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProject.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUser(RegisterViewModel newusermodel);
        Task<SignInResult> LoginUser(LoginUserViewModel uservm);
        Task LogoutUser();
        Task<IdentityResult> AddAdmin(RegisterViewModel newusermodel);
        Task<RegisterViewModel> GetUserProfileAsync(string username);
        Task<IdentityResult> EditUserProfileAsync(string username, string email);
        Task<List<RegisterViewModel>> GetAllUsersAsync(); // Fetch all users
        Task<List<RegisterViewModel>> GetAllAdmins(); // Fetch only admins
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel newusermodel)
        {
            ApplicationUser usermodel = new ApplicationUser
            {
                UserName = newusermodel.Username,
                Email = newusermodel.Email,
                Address = $"{newusermodel.Street}, {newusermodel.City}, {newusermodel.State}"
            };

            var result = await _userManager.CreateAsync(usermodel, newusermodel.Password);

            if (result.Succeeded)
            {
                // Ensure the "User" role exists before adding the user to it
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(usermodel, "User");
            }

            return result;
        }

        public async Task<SignInResult> LoginUser(LoginUserViewModel uservm)
        {
            ApplicationUser usermodel = await _userManager.FindByNameAsync(uservm.UserName);
            if (usermodel != null && await _userManager.CheckPasswordAsync(usermodel, uservm.Password))
            {
                return await _signInManager.PasswordSignInAsync(usermodel, uservm.Password, uservm.RememberMe, false);
            }

            return SignInResult.Failed;
        }

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddAdmin(RegisterViewModel newusermodel)
        {
            ApplicationUser usermodel = new ApplicationUser
            {
                UserName = newusermodel.Username,
                Email = newusermodel.Email,
                Address = $"{newusermodel.Street}, {newusermodel.City}, {newusermodel.State}"
            };

            var result = await _userManager.CreateAsync(usermodel, newusermodel.Password);

            if (result.Succeeded)
            {
                // Ensure the "Admin" role exists before adding the user to it
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await _userManager.AddToRoleAsync(usermodel, "Admin");
            }

            return result;
        }

        public async Task<RegisterViewModel> GetUserProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault();

                return new RegisterViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    RoleName = roleName
                };
            }

            return null;
        }

        public async Task<IdentityResult> EditUserProfileAsync(string username, string email)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Update the email and username (if needed)
            user.Email = email;
            user.UserName = username;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<RegisterViewModel>> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(admin => new RegisterViewModel
            {
                Username = admin.UserName,
                Email = admin.Email
            }).ToList();
        }

        public async Task<List<RegisterViewModel>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<RegisterViewModel>();

            foreach (var user in users)
            {
                // Get roles for each user
                var roles = await _userManager.GetRolesAsync(user);

                userViewModels.Add(new RegisterViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    RoleName = string.Join(", ", roles.OrderByDescending(role => role))
                });
            }

            return userViewModels;
        }
    }
}
