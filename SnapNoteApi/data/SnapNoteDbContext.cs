using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SnapNoteApi.Data{

    public class SnapNoteDbContext : IdentityDbContext<User> { // Inherit from IdentityDbContext<User> to use Identity
    public SnapNoteDbContext(DbContextOptions<SnapNoteDbContext> options) : base(options) {} // 

    public DbSet <Post> Posts { get; set; } // DbSet for the Post model
    public DbSet <Comment> Comments { get; set; } // DbSet for the Comment model
    }
   
}
 


