 
using DoToList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace StudyTracker.Controllers
{
    [Authorize] // ?? Ensures only authenticated users can access this controller
    public class HomeController : Controller
    {
        private static readonly List<StudySession> _sessions = new List<StudySession>();
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID

            var studyTime = _sessions.Where(s => s.UserId == userId && s.Type == "Study").Sum(s => s.Duration);
            var breakTime = _sessions.Where(s => s.UserId == userId && s.Type == "Break").Sum(s => s.Duration);
            var otherTime = 1440 - (studyTime + breakTime);

            var model = new StudyTrackerViewModel
            {
                StudyTime = studyTime,
                BreakTime = breakTime,
                OtherTime = otherTime,
                Sessions = _sessions.Where(s => s.UserId == userId).ToList() // Show only user's sessions
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddSession(StudySession session)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(); // Ensure the user is logged in

            if (session.StartTime >= session.EndTime)
            {
                ModelState.AddModelError("", "End time must be after start time.");
                return RedirectToAction("Index");
            }

            session.UserId = userId; // Assign the logged-in user ID to the session
            _sessions.Add(session);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSession(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = _sessions.FirstOrDefault(s => s.Id == id && s.UserId == userId); // Ensure user owns session

            if (session != null)
            {
                _sessions.Remove(session);
            }

            return RedirectToAction("Index");
        }
    }
}
