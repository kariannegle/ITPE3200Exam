using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnapNoteApi.Data;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly SnapNoteDbContext _context;

    public CommentsController(SnapNoteDbContext context)
    {
        _context = context;
    }

    // POST: api/Comments
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Comment>> CreateComment(CommentCreate model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = new Comment
        {
            Text = model.Text,
            CreatedAt = DateTime.UtcNow,
            PostId = model.PostId,
            UserId = userId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Optionally, return the created comment with user details
        comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == comment.Id);

        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }

    // PUT: api/Comments/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id,CommentUpdate model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        // Check if the current user is the owner of the comment
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (comment.UserId != userId)
        {
            return Forbid();
        }

        comment.Text = model.Text;

        _context.Entry(comment).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Comments/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        // Check if the current user is the owner of the comment
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (comment.UserId != userId)
        {
            return Forbid();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
