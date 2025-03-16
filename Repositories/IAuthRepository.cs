using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;

namespace FrightNight.Repositories
{
    public interface IAuthRepository
    {
        Task<IEnumerable<User>> GetUser(string Account);
        void ApplyUser(User user);
        Task UpdateAccount(User request);

    }
}