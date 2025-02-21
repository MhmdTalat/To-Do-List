using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Controllers
{
    public class ServiceController : Controller
    {
        // Existing Index action
        public IActionResult Index()
        {
            return View();
        }

        // Existing ServicePage action
        public IActionResult ServicePage()
        {
            return View();
        }

        // New InformationPage action
        public IActionResult InformationPage()
        {
            return View();
        }
    }
}
