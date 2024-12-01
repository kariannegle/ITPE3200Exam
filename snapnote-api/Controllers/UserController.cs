using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
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

        [HttpGet]
        [Route("Settings")]
        public IActionResult Settings()
        {
            try
            {
                _logger.LogInformation("Accessed Settings page.");
                var model = new EditProfileViewModel();
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the settings page.");
                return StatusCode(500, new { message = "An error occurred while accessing the settings page." });
            }
        }

        [HttpPost]
        [Route("Settings")]
        public async Task<IActionResult> UpdateSettings(EditProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for settings update.");
                    return BadRequest(ModelState);
                }

                var user = await _userRepository.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found during settings update.");
                    return Unauthorized(new { message = "User not found." });
                }

                // Update username
                if (user.UserName != model.Username)
                {
                    if (string.IsNullOrEmpty(model.Username))
                    {
                        ModelState.AddModelError(string.Empty, "Username cannot be empty.");
                        return BadRequest(ModelState);
                    }
                    var setUsernameResult = await _userRepository.SetUserNameAsync(user, model.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to update username for user {UserId}", user.Id);
                        foreach (var error in setUsernameResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }

                // Update email
                if (user.Email != model.Email)
                {
                    if (string.IsNullOrEmpty(model.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Email cannot be empty.");
                        return BadRequest(ModelState);
                    }
                    var setEmailResult = await _userRepository.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to update email for user {UserId}", user.Id);
                        foreach (var error in setEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }

                // Change password
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    if (string.IsNullOrEmpty(model.CurrentPassword))
                    {
                        ModelState.AddModelError(string.Empty, "Current password is required.");
                        return BadRequest(ModelState);
                    }

                    var changePasswordResult = await _userRepository.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to change password for user {UserId}", user.Id);
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }

                await _userRepository.RefreshSignInAsync(user);
                _logger.LogInformation("Settings updated successfully for user {UserId}", user.Id);
                return Ok(new { message = "Settings updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating settings for user.");
                return StatusCode(500, new { message = "An error occurred while updating settings." });
            }
        }

        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    // If no userId is provided, show the current user's profile
                    userId = await _userRepository.GetUserIdAsync(User);
                    if (userId == null)
                    {
                        _logger.LogWarning("No user is logged in and no userId was provided.");
                        return Unauthorized(new { message = "User not logged in." });
                    }
                }

                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return NotFound(new { message = "User not found." });
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

                _logger.LogInformation("Displaying profile for user {UserId}.", userId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while displaying the profile for user {UserId}.", userId);
                return StatusCode(500, new { message = "An error occurred while displaying the profile." });
            }
        }
    }
}