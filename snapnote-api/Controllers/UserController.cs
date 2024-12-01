using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IPostRepository postRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        // GET: api/user/settings
        [HttpGet("settings")]
        public IActionResult Settings()
        {
            try
            {
                _logger.LogInformation("Accessed user settings.");
                var model = new EditProfileViewModel(); // Ideally, fetch the user's settings using the repository.
                return Ok(model); // Return JSON data instead of rendering a view.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing user settings.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // PUT: api/user/settings
        [HttpPut("settings")]
        public async Task<IActionResult> Settings([FromBody] EditProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userRepository.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                // Update user settings
                if (user.UserName != model.Username)
                {
                    if (string.IsNullOrEmpty(model.Username))
                    {
                        return BadRequest(new { message = "Username cannot be null or empty" });
                    }

                    var setUsernameResult = await _userRepository.SetUserNameAsync(user, model.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        return BadRequest(setUsernameResult.Errors);
                    }
                }

                await _userRepository.RefreshSignInAsync(user);
                _logger.LogInformation("Settings updated successfully for user {UserId}", user.Id);
                return Ok(new { message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user settings.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET: api/user/{userId}/profile
        [HttpGet("{userId}/profile")]
        public async Task<IActionResult> Profile(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    // If no userId is provided, attempt to show the current user's profile.
                    userId = await _userRepository.GetUserIdAsync(User);
                    if (userId == null)
                    {
                        _logger.LogWarning("No user is logged in, and no userId was provided.");
                        return Unauthorized(new { message = "User is not logged in" });
                    }
                }

                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return NotFound(new { message = "User not found" });
                }

                var posts = await _postRepository.GetPostsByUserIdAsync(userId);
                _logger.LogInformation("Fetched {PostCount} posts for user {UserId}", posts.Count(), userId);

                var model = new UserProfileViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Posts = posts.ToList(),
                    ProfilePictureUrl = "/images/default-profile.png" // Optional property
                };

                _logger.LogInformation("Returning profile for user {UserId}.", userId);
                return Ok(model); // Return profile data as JSON for the frontend to render.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the profile for user {UserId}.", userId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
