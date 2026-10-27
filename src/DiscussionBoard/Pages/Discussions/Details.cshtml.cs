using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions;

public class DetailsModel : PageModel
{
    private readonly DiscussionThreadService _threadService;
    private readonly PostService _postService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DetailsModel(DiscussionThreadService threadService, PostService postService, UserManager<ApplicationUser> userManager)
    {
        _threadService = threadService;
        _postService = postService;
        _userManager = userManager;
    }

    public DiscussionThread Thread { get; set; }
    public IReadOnlyList<PostTreeBuilder.Node> Posts { get; set; } = new List<PostTreeBuilder.Node>();

    [BindProperty]
    public int ThreadId { get; set; }

    [BindProperty]
    public string NewPostContent { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Thread = await _threadService.GetAsync(id);

        if (Thread == null)
        {
            return NotFound();
        }
        var threadPosts = await _postService.GetForThreadAsync(id);

        Posts = PostTreeBuilder.Build(threadPosts);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPostContent))
        {
            return RedirectToPage(new { id = ThreadId });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var newPost = new Post
        {
            DiscussionThreadId = ThreadId,
            Content = NewPostContent,
            ApplicationUserId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow,

            Title = "Reply"
        };

        await _postService.AddAsync(newPost);

        return RedirectToPage(new { id = ThreadId });
    }
}
