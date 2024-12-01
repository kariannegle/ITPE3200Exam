using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using NoteApp.Models;
using NoteApp.Repositories;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpOptions("register")]
        public IActionResult PreflightForRegister()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Ok();
}
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                _logger.LogInformation("Received registration request for username: {Username}", model.Username);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid registration model state for user: {Username}", model.Username);
                    return BadRequest(ModelState);
                }

                var result = await _accountRepository.RegisterAsync(model);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully with username: {Username}", model.Username);
                    return Ok(new { message = "Registration successful" });
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Registration error for user {Username}: {Error}", model.Username, error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user: {Username}", model.Username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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

                    // Log the invalid login attempt
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
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountRepository.LogoutAsync();
                _logger.LogInformation("User logged out successfully.");
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
