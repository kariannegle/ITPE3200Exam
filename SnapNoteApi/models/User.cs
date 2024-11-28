using Microsoft.AspNetCore.Identity;

public class User : IdentityUser {
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
}