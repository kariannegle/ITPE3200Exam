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
    [Route("[controller]")]
    public class PostController : Controller
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

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postRepository.GetAllPostsAsync();
                _logger.LogInformation("Fetched all posts");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all posts.");
                return StatusCode(500, new { message = "An error occurred while fetching all posts." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(Post post, IFormFile image)
        {
            try
            {
                if (post == null)
                {
                    _logger.LogWarning("Invalid post object provided.");
                    return BadRequest("Invalid post object.");
                }

                if (image != null && image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        post.ImageData = ms.ToArray(); // Save image as byte array in the database
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
                return Ok(new { message = "Post created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a post.");
                return StatusCode(500, new { message = "An error occurred while creating a post." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided for deletion: {PostId}", id);
                    return BadRequest("Invalid post ID.");
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound();
                }

                if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to delete post {PostId}", id);
                    return Forbid();
                }

                await _postRepository.DeletePostAsync(id);
                _logger.LogInformation("Post {PostId} deleted successfully by user {UserId}", id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(new { message = "Post deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting post {PostId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the post." });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided for update: {PostId}", id);
                    return BadRequest("Invalid post ID.");
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound();
                }

                if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to update post {PostId}", id);
                    return Forbid();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching post {PostId} for update", id);
                return StatusCode(500, new { message = "An error occurred while fetching the post for update." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost(int id, Post updatedPost, IFormFile? newImage)
        {
            try
            {
                if (id <= 0 || updatedPost == null)
                {
                    _logger.LogWarning("Invalid post ID or updated post provided for update.");
                    return BadRequest("Invalid data.");
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound();
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
                return Ok(new { message = "Post updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating post {PostId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the post." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment(Comment comment, string returnUrl)
        {
            try
            {
                if (comment == null)
                {
                    _logger.LogWarning("Invalid comment object provided.");
                    return BadRequest("Invalid comment.");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    _logger.LogWarning("User ID not found.");
                    return Forbid();
                }

                // Fetch the Post object from the database using the PostId
                var post = await _postRepository.GetPostByIdAsync(comment.PostId);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", comment.PostId);
                    return NotFound("Post not found.");
                }
                // Initialize the Comment object with the Post object
                comment.Post = post;
                comment.UserId = userId;
                comment.Username = User.Identity?.Name ?? "Unknown";
                comment.CreatedAt = DateTime.Now;

                await _commentRepository.AddCommentAsync(comment);
                _logger.LogInformation("Comment added successfully by user {UserId}", comment.UserId);
                
                // Redirect based on the returnUrl
                if (string.Equals(returnUrl, "ViewPost", StringComparison.OrdinalIgnoreCase))
                {
                    // Redirect back to the ViewPost action with the post ID
                    return RedirectToAction("ViewPost", new { id = comment.PostId });
                }
                else
                {
                    // Default redirection to Index
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a comment.");
                return StatusCode(500, new { message = "An error occurred while adding the comment." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid comment ID provided for deletion: {CommentId}", id);
                    return BadRequest("Invalid comment ID.");
                }

                var comment = await _commentRepository.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found for ID {CommentId}", id);
                    return NotFound();
                }

                if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to delete comment {CommentId}", id);
                    return Forbid();
                }

                await _commentRepository.DeleteCommentAsync(id);
                _logger.LogInformation("Comment {CommentId} deleted successfully by user {UserId}", id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(new { message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting comment {CommentId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the comment." });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateComment")]
        public async Task<IActionResult> UpdateComment(int id, Comment updatedComment)
        {
            try
            {
                if (id <= 0 || updatedComment == null)
                {
                    _logger.LogWarning("Invalid comment ID or updated comment provided for update.");
                    return BadRequest("Invalid data.");
                }

                var comment = await _commentRepository.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found for ID {CommentId}", id);
                    return NotFound();
                }

                if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to update comment {CommentId}", id);
                    return Forbid();
                }

                comment.Content = updatedComment.Content;
                await _commentRepository.UpdateCommentAsync(comment);
                _logger.LogInformation("Comment {CommentId} updated successfully by user {UserId}", id, comment.UserId);
                return Ok(new { message = "Comment updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating comment {CommentId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the comment." });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("UpdateComment")]
        public async Task<IActionResult> UpdateComment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid comment ID provided for update: {CommentId}", id);
                    return BadRequest("Invalid comment ID.");
                }

                var comment = await _commentRepository.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found for ID {CommentId}", id);
                    return NotFound();
                }

                if (comment.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    _logger.LogWarning("User not authorized to update comment {CommentId}", id);
                    return Forbid();
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching comment {CommentId} for update", id);
                return StatusCode(500, new { message = "An error occurred while fetching the comment for update." });
            }
        }

        [HttpGet]
        [Route("ViewPost")]
        public async Task<IActionResult> ViewPost(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid post ID provided for viewing: {PostId}", id);
                    return BadRequest("Invalid post ID.");
                }

                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Post not found for ID {PostId}", id);
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while viewing post {PostId}", id);
                return StatusCode(500, new { message = "An error occurred while viewing the post." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                _logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception occurred.");
            }

            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return Ok(errorViewModel);
        }
    }
}