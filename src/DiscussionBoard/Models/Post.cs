using System.ComponentModel.DataAnnotations;

namespace DiscussionBoard.Models;

public class Post : DiscussionBase
{
    // Foreign Key matching EF Core conventions
    public int DiscussionThreadId { get; set; }
    public DiscussionThread DiscussionThread { get; set; }

    // Self-referencing relationship for replies (Nullable int allows root posts)
    public int? ParentPostId { get; set; }
    public Post ParentPost { get; set; }

    // Relationship: One Post can have many Reply Posts
    public ICollection<Post> Replies { get; set; } = new List<Post>();
}
