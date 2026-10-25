using DiscussionBoard.Data;
using DiscussionBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Pages.Forums;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Forum Forum { get; set; }
    public List<DiscussionThread> Threads { get; set; } = new List<DiscussionThread>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Forum = await _context.Forums
            .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

        if (Forum == null)
        {
            return NotFound();
        }

        Threads = await _context.DiscussionThreads
            .Where(t => t.ForumId == id && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return Page();
    }
}


