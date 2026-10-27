using DiscussionBoard.Data;
using DiscussionBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscussionBoard.Services;

public class DiscussionThreadService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DiscussionThreadService>? _logger;

    public DiscussionThreadService(ApplicationDbContext context)
    {
        _context = context;
        _logger = null;
    }

    public DiscussionThreadService(ApplicationDbContext context, ILogger<DiscussionThreadService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DiscussionThread?> GetAsync(int id)
    {
        return await _context.DiscussionThreads
            .Include(t => t.ApplicationUser)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task<IReadOnlyList<DiscussionThread>> GetForForumAsync(int forumId)
    {
        return await _context.DiscussionThreads
            .Where(t => t.ForumId == forumId && !t.IsDeleted)
            .OrderByDescending(t => t.Id) //
            .ToListAsync();
    }

    public async Task AddAsync(DiscussionThread thread)
    {
        try
        {
            thread.CreatedAt = DateTimeOffset.UtcNow;
            _context.DiscussionThreads.Add(thread);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while saving thread titled {Title}.", thread.Title);
            throw;
        }
    }

    public async Task UpdateAsync(DiscussionThread thread)
    {
        var existingThread = await _context.DiscussionThreads
            .FirstOrDefaultAsync(t => t.Id == thread.Id && !t.IsDeleted);

        if (existingThread == null)
        {
            throw new KeyNotFoundException($"Thread with ID {thread.Id} not found.");
        }

        existingThread.Title = thread.Title;
        existingThread.Content = thread.Content;

        await _context.SaveChangesAsync();
    }
}
