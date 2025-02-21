using ECommerceProject.Data;
 
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

public class FeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FeedbackService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Submit feedback
    public async Task<bool> SubmitFeedbackAsync(FeedbackViewModel feedbackModel, ApplicationUser currentUser)
    {
        if (currentUser == null || feedbackModel == null)
        {
            return false;
        }

        var feedback = new ApplicationUser
        {
            Email = feedbackModel.Email,
            Comment = feedbackModel.Comment,
            PhoneNumber = feedbackModel.PhoneNumber,
            DateSubmitted = DateTime.Now
            // Link feedback to user can be added here if necessary
        };

        _context.Users.Add(feedback);
        await _context.SaveChangesAsync();
        return true;
    }

    // Retrieve list of feedbacks
    public IQueryable<FeedbackViewModel> GetFeedbackList()
    {
        return _context.Users
            .Select(f => new FeedbackViewModel
            {
                Email = f.Email,
                Comment = f.Comment,
                DateSubmitted = f.DateSubmitted
            })
            .OrderByDescending(f => f.DateSubmitted);
    }


    public async Task<bool> DeleteFeedbackAsync(string userId)
    {
        var feedback = await _context.Users.FindAsync(userId);
        if (feedback == null)
        {
            return false;
        }

        _context.Users.Remove(feedback);
        await _context.SaveChangesAsync();
        return true;
    }

}
