using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace DiscussionBoard.Models;

public abstract class DiscussionBase : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
}
