// AccountController.cs
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAccount")]
        public IActionResult GetAccount()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.RegisterAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User registered successfully with username: {Username}", model.Username);
                        return RedirectToAction("Index", "Post");
                    }

                    // Log validation errors from the registration process
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
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user: {Username}", model.Username);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
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
                        return RedirectToAction("Index", "Post");
                    }

                    // Log the invalid login attempt
                    _logger.LogWarning("Invalid login attempt for user: {Username}", model.Username);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    _logger.LogWarning("Invalid login model state for user: {Username}", model.Username);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", model.Username);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _accountRepository.LogoutAsync();
                _logger.LogInformation("User logged out successfully.");
                return RedirectToAction("Index", "Post");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return RedirectToAction("Error", "Error");
            }
        }
    }
}