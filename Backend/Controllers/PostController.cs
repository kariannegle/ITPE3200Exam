// Controllers/PostController.cs
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NoteApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _postRepository.GetPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching posts.");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching posts." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new { success = false, message = "Post not found." });
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the post with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching the post." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] string Content, [FromForm] string Username, [FromForm] IFormFile? Image)
        {
            try
            {
                if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(Username))
                {
                    return BadRequest(new { success = false, message = "Content and Username are required." });
                }

                var post = new Post
                {
                    Content = Content,
                    Username = Username,
                    CreatedAt = DateTime.UtcNow
                };

                if (Image != null && Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        post.ImageData = memoryStream.ToArray();
                    }
                }

                await _postRepository.CreatePostAsync(post);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a post.");
                return StatusCode(500, new { success = false, message = "An error occurred while creating a post." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] string Content, [FromForm] string Username, [FromForm] IFormFile? Image)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new { success = false, message = "Post not found." });
                }

                post.Content = Content;
                post.Username = Username;

                if (Image != null && Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        post.ImageData = memoryStream.ToArray();
                    }
                }

                await _postRepository.UpdatePostAsync(post);
                return Ok(new { success = true, message = "Post updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the post with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "An error occurred while updating the post." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _postRepository.DeletePostAsync(id);
                return Ok(new { success = true, message = "Post deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the post with ID {Id}", id);
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the post." });
            }
        }
    }
}