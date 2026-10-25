using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions;

public class DetailsModel : PageModel
{
    private readonly DiscussionThreadService _threadService;
    private readonly PostService _postService;

    public DetailsModel(DiscussionThreadService threadService, PostService postService)
    {
        _threadService = threadService;
        _postService = postService;
    }


    public DiscussionThread Thread { get; set; }
    public IReadOnlyList<PostTreeBuilder.Node> Posts { get; set; } = new List<PostTreeBuilder.Node>();



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
}
