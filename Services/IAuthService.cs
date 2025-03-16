using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public interface IAuthService
    {
        Task<ApiRes<Auth>> GetUser(AuthRequest request);
        Task<User> ApplyUser(AuthRequest request);
        Task<ApiRes<Auth>> UpdateAccount(AuthChangeRequest request);
        Task<ApiRes<bool>> ValidateToken(string token);
    }
}