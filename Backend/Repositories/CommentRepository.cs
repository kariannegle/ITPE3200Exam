using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteApp.Data;
using NoteApp.Models;

public class CommentRepository : ICommentRepository
{
    private readonly NoteAppContext _context;

    public CommentRepository(NoteAppContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }
}