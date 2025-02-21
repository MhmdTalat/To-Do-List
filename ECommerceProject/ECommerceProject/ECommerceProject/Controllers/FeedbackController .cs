using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class FeedbackController : Controller
{
    private readonly FeedbackService _feedbackService;
    private readonly UserManager<ApplicationUser> _userManager;

    public FeedbackController(FeedbackService feedbackService, UserManager<ApplicationUser> userManager)
    {
        _feedbackService = feedbackService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult SubmitFeedback()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> SubmitFeedback(FeedbackViewModel feedbackModel)
    {
        // Get current user
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            // Submit the feedback using the service
            bool feedbackSubmitted = await _feedbackService.SubmitFeedbackAsync(feedbackModel, currentUser);
            if (feedbackSubmitted)
            {
                return RedirectToAction("ThankYou");
            }
        }

        // If the model state is invalid, redisplay the form
        return View(feedbackModel);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult FeedbackList()
    {
        var feedbacks = _feedbackService.GetFeedbackList().ToList();
        return View(feedbacks);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFeedback(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Invalid feedback ID.");
        }

        bool isDeleted = await _feedbackService.DeleteFeedbackAsync(userId);
        if (isDeleted)
        {
            return RedirectToAction("FeedbackList");
        }

        return NotFound("Feedback not found.");
    }

    public IActionResult ThankYou()
    {
        return View();
    }
}
