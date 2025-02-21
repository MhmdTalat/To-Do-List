using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProject.Controllers
{
    public class TraderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TraderController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/AddAdmin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Addadnewtrader()
        {
            return View();
        }

        // POST: /Account/AddAdmin
        [HttpPost]
        public async Task<IActionResult> Addadnewtrader(RegisterViewModel newusermodel, bool isAdmin = false)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("AccessDenied", "Home"); // Redirect to an "Access Denied" page or another appropriate page
            }

            if (ModelState.IsValid)
            {
                ApplicationUser usermodel = new ApplicationUser
                {
                    UserName = newusermodel.Username,
                    Email = newusermodel.Email,
                    Address = $"{newusermodel.Street}, {newusermodel.City}, {newusermodel.State}"
                };

                IdentityResult result = await _userManager.CreateAsync(usermodel, newusermodel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usermodel, "Trader");
                    await _signInManager.SignInAsync(usermodel, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erroritem.Description);
                    }
                }
            }

            return View(newusermodel);
        }

        // GET: /Trader/GetAllAdmins
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminViewModels = admins.Select(admin => new RegisterViewModel
            {
                Username = admin.UserName,
                Email = admin.Email
            }).ToList();
            return View(adminViewModels);
        }
    }
}
