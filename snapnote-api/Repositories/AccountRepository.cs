using Microsoft.AspNetCore.Identity;
using NoteApp.Models;

namespace NoteApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully with username: {Username}", model.Username);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning("User creation error: {ErrorDescription}", error.Description);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user: {Username}", model.Username);
                throw;  // Let the controller handle it
            }
        }


        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    throw new ArgumentException("Username and password must not be null or empty.");
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in successfully with username: {Username}", model.Username);
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for username: {Username}", model.Username);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", model.Username);
                throw; // Re-throwing the exception to let the controller handle it if needed
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                throw; // Re-throwing the exception to let the controller handle it if needed
            }
        }
    }
}
