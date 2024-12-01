// AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using NoteApp.Models;
using NoteApp.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowReactApp")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        // GET api/account/accountinfo (for demonstration)
        [HttpGet("accountinfo")]
        public IActionResult GetAccount()
        {
            // Returning a placeholder view or account data if needed
            return Ok(new { message = "Account Info" });
        }

        // POST api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.RegisterAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User registered successfully with username: {Username}", model.Username);
                        return Ok(new { message = "User registered successfully" });
                    }

                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning("Registration error: {ErrorDescription}", error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid registration model state for user: {Username}", model.Username);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user: {Username}", model.Username);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in successfully with username: {Username}", model.Username);
                        return Ok(new { message = "Login successful" });
                    }

                    _logger.LogWarning("Invalid login attempt for user: {Username}", model.Username);
                    return Unauthorized(new { message = "Invalid login attempt" });
                }
                else
                {
                    _logger.LogWarning("Invalid login model state for user: {Username}", model.Username);
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", model.Username);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountRepository.LogoutAsync();
                _logger.LogInformation("User logged out successfully.");
                return Ok(new { message = "User logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
