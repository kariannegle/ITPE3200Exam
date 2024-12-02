using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;
using System;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            await _commentRepository.DeleteCommentAsync(id);
            return Ok(new { success = true, message = "Comment deleted successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while deleting the comment." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment updatedComment)
    {
        if (updatedComment == null || string.IsNullOrEmpty(updatedComment.Content))
        {
            return BadRequest("Invalid comment data.");
        }

        try
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound(new { success = false, message = "Comment not found." });
            }

            comment.Content = updatedComment.Content;
            await _commentRepository.UpdateCommentAsync(comment);
            return Ok(new { success = true, message = "Comment updated successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while updating the comment." });
        }
    }
}