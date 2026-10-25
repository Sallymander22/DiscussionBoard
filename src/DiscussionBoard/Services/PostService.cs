using DiscussionBoard.Data;
using DiscussionBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscussionBoard.Services;

public class PostService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PostService>? _logger;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
        _logger = null;
    }

    public PostService(ApplicationDbContext context, ILogger<PostService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Post>> GetForThreadAsync(int threadId)
    {
        return await _context.Posts
            .Include(p => p.ApplicationUser)
            .Include(p => p.Replies)
            .Where(p => p.DiscussionThreadId == threadId && !p.IsDeleted)
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task AddAsync(Post post)
    {
        try
        {
            post.CreatedAt = DateTimeOffset.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error adding comment post.");
            throw;
        }
    }
}
