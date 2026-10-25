using System.ComponentModel.DataAnnotations;

namespace DiscussionBoard.Models;

public class DiscussionThread : DiscussionBase
{
    // Foreign Key matching EF Core conventions
    public int ForumId { get; set; }

    // Navigation property back to the Forum
    public Forum Forum { get; set; }

    // Relationship: One Thread has many Posts
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
