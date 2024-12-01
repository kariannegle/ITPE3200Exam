using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NoteApp.Models;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // GET: api/error/unhandled
        [HttpGet("unhandled")]
        public IActionResult Error()
        {
            // Get the exception details
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                _logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception occurred.");
            }

            return StatusCode(500, new
            {
                message = "An unexpected error occurred. Please try again later.",
                requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
