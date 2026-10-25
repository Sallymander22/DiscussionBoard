using System.ComponentModel.DataAnnotations;
using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions;

[Authorize] // Enforces that users must log in to create a thread
public class CreateModel : PageModel
{
    private readonly DiscussionThreadService _threadService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateModel(DiscussionThreadService threadService, UserManager<ApplicationUser> userManager)
    {
        _threadService = threadService;
        _userManager = userManager;
    }

    [BindProperty]
    public int ForumId { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title must be between 5 and 100 characters.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "Content must be between 10 and 5000 characters.", MinimumLength = 10)]
        public string Content { get; set; }
    }

    public IActionResult OnGet(int forumId)
    {
        ForumId = forumId;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Get currently authenticated student user details
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var newThread = new DiscussionThread
        {
            ForumId = ForumId,
            Title = Input.Title,
            Content = Input.Content,
            ApplicationUserId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _threadService.AddAsync(newThread);

        // Redirect back to details tab to display the freshly added item
        return RedirectToPage("/Forums/Details", new { id = ForumId });
    }
}
