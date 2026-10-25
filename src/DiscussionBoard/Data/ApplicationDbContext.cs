using DiscussionBoard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Forum> Forums { get; set; }
    public DbSet<DiscussionThread> DiscussionThreads { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Post>()
            .HasOne(p => p.ParentPost)
            .WithMany(p => p.Replies)
            .HasForeignKey(p => p.ParentPostId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents cascading delete cycles
    }
}

