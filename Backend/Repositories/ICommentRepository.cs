using System.Collections.Generic;
using System.Threading.Tasks;
using NoteApp.Models;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    Task AddCommentAsync(Comment comment);
    // Other method signatures...
}