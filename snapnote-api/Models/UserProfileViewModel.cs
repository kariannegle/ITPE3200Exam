using System.ComponentModel.DataAnnotations;

namespace NoteApp.Models
{
    public class UserProfileViewModel  // View model for user profile information
    {
        public UserProfileViewModel() { }
        public UserProfileViewModel(string userId, string username, List<Post> posts)
        {
            UserId = userId;
            Username = username;
            Posts = posts;
        }
        [Required(ErrorMessage = "User ID is required.")]
        [MaxLength(50, ErrorMessage = "User ID cannot exceed 50 characters.")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public required string Username { get; set; }

        [MaxLength(200, ErrorMessage = "Profile picture URL cannot exceed 200 characters.")]
        [Url(ErrorMessage = "The profile picture URL must be a valid URL.")]
        public string? ProfilePictureUrl { get; set; }

        public List<Post> Posts { get; set; } = new();

        
    }
}
