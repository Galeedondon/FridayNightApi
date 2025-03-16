using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Data;
using FrightNight.Models;
using Microsoft.EntityFrameworkCore;

namespace FrightNight.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Comments> Create(Comments request)
        {
            var entry = await _context.Comments.AddAsync(request);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task Update(Comments request)
        {
            _context.Comments.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comments>> Get(int Id)
        {
            return await _context.Comments.Where(m => m.MemoryId == Id && m.DeleteAt == null && m.DeleteDate == null).ToListAsync();
        }
        public async Task<Comments> Delete(int Id, string deleteAt)
        {
            var comments = await _context.Comments.FirstOrDefaultAsync(m => m.Id == Id && m.DeleteAt == null && m.DeleteDate == null);

            if (comments != null)
            {
                comments.DeleteAt = deleteAt; // 設置 DeleteAt
                comments.DeleteDate = DateTime.Now; // 設置 DeleteDate 為當前時間
 
                await _context.SaveChangesAsync(); // 保存更改
            }

            return comments;
        }
    }
}