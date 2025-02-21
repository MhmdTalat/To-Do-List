using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel newusermodel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUser(newusermodel);
                if (result.Succeeded)
                {
                    await _accountService.LoginUser(new LoginUserViewModel { UserName = newusermodel.Username, Password = newusermodel.Password });
                    return RedirectToAction("Logout", "Account");
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel uservm)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginUser(uservm);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "User name or password Wrong");
            }
            return View(uservm);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUser();
            return RedirectToAction("Login");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Addadmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Addadmin(RegisterViewModel newusermodel)
        {
            //if (!User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("AccessDenied", "Home");
            //}

            if (ModelState.IsValid)
            {
                var result = await _accountService.AddAdmin(newusermodel);
                if (result.Succeeded)
                {
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _accountService.GetUserProfileAsync(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            // Retrieve the current user's profile based on the username (from User.Identity.Name)
            var user = await _accountService.GetUserProfileAsync(User.Identity.Name);

            // If the user is not found, return a NotFound result
            if (user == null)
            {
                return NotFound();
            }

            // Create and populate the EditUserProfileViewModel
            var model = new EditUserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email
            };

            // Return the view with the populated model
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel model)
        {
            // Ensure the model state is valid
            if (ModelState.IsValid)
            {
                // Attempt to update the user profile through the account service
                var result = await _accountService.EditUserProfileAsync(model.Username, model.Email);

                if (result.Succeeded)
                {
                    // If successful, redirect to the Profile page with the updated username
                    return RedirectToAction("Profile", new { id = model.Username });
                }

                // If there are errors, add them to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If model state is invalid or update failed, return the current model back to the view
            return View(model);
        }


        // Other actions...
         


        public async Task<IActionResult> ListUsers()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users); // Pass the list of users to the view
        }
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _accountService.GetAllAdmins();
            return View(admins);
        }



       


    }
}
