public class Post {
    public int Id { get; set; } // Primary key for the post
    public string Content { get; set; } // Text content of the post
    public string ImageUrl { get; set; } // URL to the uploaded image
    public DateTime CreatedAt { get; set; } // Date and time of post creation
    public string UserId { get; set; } // Foreign key to the User who created the post
    public User User { get; set; } // Navigation property to the User who created the post
    public ICollection<Comment> Comments { get; set; } // Navigation property to the comments on the post
}