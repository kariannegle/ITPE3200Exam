using System.Collections.Generic;
using System.Threading.Tasks;
using NoteApp.Models;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task<Comment> GetCommentByIdAsync(int id);
    Task AddCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
    Task UpdateCommentAsync(Comment comment);
    // Other method signatures...
}