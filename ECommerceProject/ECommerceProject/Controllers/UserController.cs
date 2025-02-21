using ECommerceProject.ViewModels;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerceProject.Data;

namespace ECommerceProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Register Page



        // GET: Delete User Confirmation Page
        // GET: Delete User Confirmation Page
        [HttpGet("user/delete/{username}")]
        public IActionResult Delete(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required.");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user); // Return the user to the confirmation view
        }

        [HttpPost("user/delete/confirmed/{username}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required.");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Message"] = "User deleted successfully.";
            return RedirectToAction("Index"); // Redirect back to the list of users
        }

    }
}
