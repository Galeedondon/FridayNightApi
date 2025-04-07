using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Data;
using FrightNight.Models;
using Microsoft.EntityFrameworkCore;

namespace FrightNight.Repositories
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly AppDbContext _context;

        public MemoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Memorys> Create(Memorys req)
        {
            var entry = await _context.Memorys.AddAsync(req);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Memorys> Get(int Id)
        {
            return await _context.Memorys.Where(m => m.Id == Id && m.DeleteAt == null && m.DeleteDate == null).FirstOrDefaultAsync();
        }

        public async Task<List<Memorys>> GetByDate(DateTime date)
        {
            var result = await _context.Memorys
                .Where(m => m.MemoryDate.Date == date.Date && m.DeleteAt == null && m.DeleteDate == null)
                .OrderByDescending(m => m.CreateDate) // 依據 CreateDate 排序
                .ToListAsync();

            return result;
        }

        public async Task<List<Memorys>> GetByMonth(DateTime date)
        {
            DateTime requestDate = new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime requestMonthEnd = requestDate.AddMonths(1).AddSeconds(-1); // 當月的最後一天

            var result = await _context.Memorys
                .Where(m => m.DeleteAt == null && m.DeleteDate == null && m.MemoryDate >= requestDate && m.MemoryDate <= requestMonthEnd)
                .OrderByDescending(m => m.MemoryDate) // 依據 CreateDate 排序
                .ToListAsync();


            return result;
        }


        public async Task<Memorys> Delete(int id, string deleteAt)
        {
            var memory = await _context.Memorys
                .FirstOrDefaultAsync(m => m.Id == id && m.DeleteAt == null && m.DeleteDate == null);

            if (memory != null)
            {
                // 取得圖片進行假刪除
                var picture = await _context.Pictures.FirstOrDefaultAsync(m => m.Id == memory.PicKey && m.DeleteAt == null && m.DeleteDate == null);

                memory.DeleteAt = deleteAt; // 設置 DeleteAt
                memory.DeleteDate = DateTime.Now; // 設置 DeleteDate 為當前時間
                picture.DeleteAt = deleteAt; // 設置 DeleteAt
                picture.DeleteDate = DateTime.Now; // 設置 DeleteDate 為當前時間

                await _context.SaveChangesAsync(); // 保存更改
            }

            return memory;
        }

        public async Task<string> CreatePicture(Pictures req)
        {



            try
            {
                // Ensure that CreateAt is set to the current timestamp
                req.CreateAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

                // Ensure that CreateDate is set
                req.CreateDate = DateTime.UtcNow;

                // Set the Id for the picture if not set
                req.Id = Guid.NewGuid().ToString();

                // Add the picture to the database
                await _context.Pictures.AddAsync(req);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return req.Id;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error saving picture: {ex.Message}");
                return null;
            }
        }

        public async Task<Pictures> GetPicture(string Id)
        {
            return await _context.Pictures.Where(m => m.Id == Id && m.DeleteAt == null && m.DeleteDate == null).FirstOrDefaultAsync();
        }
    }
}