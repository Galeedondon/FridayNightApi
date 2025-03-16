using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Data;
using FrightNight.Models;
using Microsoft.EntityFrameworkCore;

namespace FrightNight.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUser(string Account)
        {
            var userList = await _context.Users
                                .Where(u => u.Account == Account)
                                .ToListAsync();
            return userList;
        }
        public async void ApplyUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccount(User request)
        {
            var existingUser = await _context.Users.FindAsync(request.Id);
            if (existingUser != null)
            {
                existingUser.Password = request.Password;
                // 不需要再次附加，因為它已經被跟踪
                await _context.SaveChangesAsync();
            }
        }

    }
}