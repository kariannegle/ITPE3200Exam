using NoteApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace NoteApp.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly ILogger<FriendController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly UserManager<User> _userManager;

        public FriendController(
            ILogger<FriendController> logger, 
            IUserRepository userRepository,
            IFriendRepository friendRepository, 
            UserManager<User> userManager) 
        {
            _logger = logger;
            _userRepository = userRepository;
            _friendRepository = friendRepository;
            _userManager = userManager;
        }

        // GET: api/friend
        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    _logger.LogWarning("User is not authenticated.");
                    return Unauthorized(new { message = "User is not authenticated." });
                }

                var friends = await _friendRepository.GetFriendsByUserIdAsync(currentUser.Id);
                _logger.LogInformation("Fetched {FriendCount} friends for user {UserId}", friends.Count(), currentUser.Id);
                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching friends for user.");
                return StatusCode(500, new { message = "An error occurred while fetching friends." });
            }
        }

        // GET: api/friend/search?searchTerm={searchTerm}
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            try
            {   
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.LogWarning("Search term is null or empty.");
                    return BadRequest(new { message = "Search term cannot be empty." });
                }

                var users = await _userManager.Users
                    .Where(u => (u.UserName != null && u.UserName.Contains(searchTerm)) || 
                                (u.Email != null && u.Email.Contains(searchTerm)))
                    .ToListAsync();

                _logger.LogInformation("Fetched {UserCount} users for search term '{SearchTerm}'", users.Count, searchTerm);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for users with term: {SearchTerm}", searchTerm);
                return StatusCode(500, new { message = "An error occurred while searching for users." });
            }
        }

        // POST: api/friend/add
        [HttpPost("add")]
        public async Task<IActionResult> AddFriend(string friendId)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    _logger.LogWarning("User is not authenticated.");
                    return Unauthorized(new { message = "User is not authenticated." });
                }

                if (currentUser.Id == friendId)
                {
                    _logger.LogWarning("User {UserId} tried to add themselves as a friend.", currentUser.Id);
                    return BadRequest(new { message = "You cannot add yourself as a friend." });
                }

                var existingFriendship = await _friendRepository.GetFriendshipAsync(currentUser.Id, friendId);
                if (existingFriendship != null)
                {
                    _logger.LogWarning("User {UserId} tried to add a friend {FriendId} they are already friends with.", currentUser.Id, friendId);
                    return BadRequest(new { message = "You are already friends with this user." });
                }

                var friendUser = await _userManager.FindByIdAsync(friendId);
                if (friendUser == null)
                {
                    _logger.LogWarning("Friend user with ID {FriendId} not found.", friendId);
                    return NotFound(new { message = "Friend user not found." });
                }

                var friendship = new Friend(currentUser.Id, friendId)
                {
                    User = currentUser,
                    FriendUser = friendUser
                };
                
                await _friendRepository.AddFriendAsync(friendship);
                _logger.LogInformation("User {UserId} added friend {FriendId}", currentUser.Id, friendId);
                return Ok(new { message = "Friend added successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding friend {FriendId} for user {UserId}", friendId, _userManager.GetUserId(User));
                return StatusCode(500, new { message = "An error occurred while adding the friend." });
            }
        }

        // DELETE: api/friend/{friendId}
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> DeleteFriend(string friendId)
        {
            try
            {
                if (string.IsNullOrEmpty(friendId))
                {
                    _logger.LogWarning("Friend ID is null or empty.");
                    return BadRequest(new { message = "Invalid friend ID." });
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    _logger.LogWarning("Current user is null. User must be logged in.");
                    return Unauthorized(new { message = "User not logged in." });
                }

                await _friendRepository.DeleteFriendAsync(currentUser.Id, friendId);
                _logger.LogInformation("User {UserId} deleted friend {FriendId}", currentUser.Id, friendId);
                return Ok(new { message = "Friend deleted successfully." });
            }
            catch (Exception ex)
            {
                var userId = _userManager.GetUserId(User); // Ensure userId is available in the catch block
                _logger.LogError(ex, "An error occurred while deleting friend {FriendId} for user {UserId}", friendId, userId);
                return StatusCode(500, new { message = "An error occurred while deleting the friend." });
            }
        }
    }
}