using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NoteApp.Models;
using System.Security.Claims;
using NoteApp.Repositories;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace NoteApp.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        // GET: api/post
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _postRepository.GetAllPostsAsync();
                _logger.LogInformation("Fetched all posts.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all posts.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // POST: api/post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post, IFormFile? image)
        {
            try
            {
                if (post == null)
                {
                    _logger.LogWarning("Invalid post object provided.");
                    return BadRequest(new { message = "Invalid post object." });
                }

                if (image != null && image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        post.ImageData = ms.ToArray(); // Save image as byte array
                    }
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    _logger.LogWarning("User ID not found.");
                    return Forbid();
                }

                post.UserId = userId;
                post.Username = User.Identity?.Name ?? "Unknown";
                post.CreatedAt = DateTime.Now;

                await _postRepository.AddPostAsync(post);
                _logger.LogInformation("Post created successfully by user {UserId}", post.UserId);
                return Ok(new { message = "Post created successfully", post });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a post.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // DELETE: api/post/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided for deletion: {PostId}", id);
                    return BadRequest(new { message = "Invalid post ID." });
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound(new { message = "Post not found." });
                }

                if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to delete post {PostId}", id);
                    return Forbid();
                }

                await _postRepository.DeletePostAsync(id);
                _logger.LogInformation("Post {PostId} deleted successfully by user {UserId}", id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(new { message = "Post deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting post {PostId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // PUT: api/post/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updatedPost, IFormFile? newImage)
        {
            try
            {
                if (id <= 0 || updatedPost == null)
                {
                    _logger.LogWarning("Invalid post ID or updated post data provided.");
                    return BadRequest(new { message = "Invalid post ID or post data." });
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound(new { message = "Post not found." });
                }

                if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to update post {PostId}", id);
                    return Forbid();
                }

                if (newImage != null && newImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await newImage.CopyToAsync(ms);
                        post.ImageData = ms.ToArray(); // Save updated image as byte array
                    }
                }

                post.Content = updatedPost.Content;
                await _postRepository.UpdatePostAsync(post);
                _logger.LogInformation("Post {PostId} updated successfully by user {UserId}", id, post.UserId);
                return Ok(new { message = "Post updated successfully", post });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating post {PostId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET: api/post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided for viewing: {PostId}", id);
                    return BadRequest(new { message = "Invalid post ID." });
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound(new { message = "Post not found." });
                }

                _logger.LogInformation("Fetched post with ID {PostId}.", id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while viewing post {PostId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // POST: api/post/{postId}/comments
        [Authorize]
        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> AddComment(int postId, [FromBody] Comment comment)
        {
            try
            {
                if (comment == null)
                {
                    _logger.LogWarning("Invalid comment object provided.");
                    return BadRequest(new { message = "Invalid comment." });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    _logger.LogWarning("User ID not found.");
                    return Forbid();
                }

                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", postId);
                    return NotFound(new { message = "Post not found." });
                }

                comment.Post = post;
                comment.UserId = userId;
                comment.Username = User.Identity?.Name ?? "Unknown";
                comment.CreatedAt = DateTime.Now;

                await _commentRepository.AddCommentAsync(comment);
                _logger.LogInformation("Comment added successfully to post {PostId} by user {UserId}", postId, comment.UserId);
                return Ok(new { message = "Comment added successfully", comment });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a comment.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // DELETE: api/post/comments/{commentId}
        [Authorize]
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            try
            {
                if (commentId <= 0)
                {
                    _logger.LogWarning("Invalid comment ID provided for deletion: {CommentId}", commentId);
                    return BadRequest(new { message = "Invalid comment ID." });
                }

                var comment = await _commentRepository.GetCommentByIdAsync(commentId);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found for ID {CommentId}", commentId);
                    return NotFound(new { message = "Comment not found." });
                }

                if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to delete comment {CommentId}", commentId);
                    return Forbid();
                }

                await _commentRepository.DeleteCommentAsync(commentId);
                _logger.LogInformation("Comment {CommentId} deleted successfully by user {UserId}", commentId, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(new { message = "Comment deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting comment {CommentId}", commentId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
