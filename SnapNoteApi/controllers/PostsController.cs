using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnapNoteApi.Data;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly SnapNoteDbContext _context;

    public PostsController(SnapNoteDbContext context)
    {
        _context = context;
    }

    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
    {
        return await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    // GET: api/Posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return post;
    }

    // POST: api/Posts
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromForm] PostCreate model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var post = new Post
        {
            Content = model.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        // Handle image upload if provided
        if (model.Image != null)
        {
            // You need to implement image saving logic here
            // For simplicity, let's assume the image URL is set to a placeholder
            post.ImageUrl = "/images/placeholder.png";
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    // PUT: api/Posts/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, PostUpdate model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        // Check if the current user is the owner of the post
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.UserId != userId)
        {
            return Forbid();
        }

        post.Content = model.Content;
        // Update other fields if necessary

        _context.Entry(post).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Posts/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        // Check if the current user is the owner of the post
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.UserId != userId)
        {
            return Forbid();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
