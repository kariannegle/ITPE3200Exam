using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetComments(int postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] Comment comment)
    {
        if (comment == null || string.IsNullOrEmpty(comment.Content))
        {
            return BadRequest("Invalid comment data.");
        }

        comment.CreatedAt = DateTime.UtcNow;
        comment.Username = "Cool"; // Hardcoded as per requirement

        await _commentRepository.AddCommentAsync(comment);
        return Ok(comment);
    }
}