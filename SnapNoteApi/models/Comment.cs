public class Comment{
    public int Id { get; set; } // Primary key for the comment
    public string Text { get; set; } // Text content of the comment
    public DateTime CreatedAt { get; set; } // Date and time of comment creation
    public int PostId { get; set; } // Foreign key to the Post the comment is on
    public Post Post { get; set; } // Navigation property to the Post the comment is on
    public string UserId { get; set; } // Foreign key to the User who created the comment
    public User User { get; set; } // Navigation property to the User who created the comment
}