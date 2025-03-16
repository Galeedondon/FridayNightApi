using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FrightNight.Models;
using FrightNight.Repositories;
using Microsoft.IdentityModel.Tokens;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<ApiRes<Auth>> GetUser(AuthRequest request)
        {
            var user = await _repository.GetUser(request.Account);
            if (user == null || !VerifyPassword(request?.Password, user.FirstOrDefault()?.Password))
            {
                return new ApiRes<Auth>
                {
                    Result = false,
                    Msg = "帳號或密碼錯誤"
                };
            }
            var token = GenerateJwtToken(user.FirstOrDefault());

            var auth = new Auth();
            auth.UserId = request.Account;
            auth.Token = token;
            auth.ValidDate = DateTime.Now.AddDays(1);

            return new ApiRes<Auth>
            {
                Result = true,
                Data = auth,
                Msg = "登入成功"
            };
        }

        public async Task<ApiRes<Auth>> UpdateAccount(AuthChangeRequest request)
        {
            var user = await _repository.GetUser(request.Account);
            if (user == null || !VerifyPassword(request?.OldPassword, user.FirstOrDefault()?.Password))
            {
                return new ApiRes<Auth>
                {
                    Result = false,
                    Msg = "帳號或密碼錯誤"
                };
            }
            var passwordHash = HashPassword(request.NewPassword);

            var newUser = new User
            {
                Id = user.FirstOrDefault().Id,
                Account = request.Account,
                Password = passwordHash
            };

            await _repository.UpdateAccount(newUser);

            return new ApiRes<Auth>
            {
                Result = true,
                Msg = "修改成功"
            };
        }


        private bool VerifyPassword(string password, string passwordHash)
        {
            // 這裡使用 SHA256 進行驗證，實際上應考慮使用更安全的加密方式，如 PBKDF2、bcrypt 等
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                var hashString = Convert.ToBase64String(hashBytes);
                return hashString == passwordHash;
            }
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Account)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Account) // 根據需求添加更多的聲明
            }),
                Expires = DateTime.UtcNow.AddHours(1), // 設置過期時間
                SigningCredentials = signingCredentials // 使用正確的 SigningCredentials
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<User> ApplyUser(AuthRequest request)
        {
            var userProfile = await _repository.GetUser(request.Account);
            if (userProfile.Count() >= 1)
            {
                throw new ApplicationException("帳號已存在");
            }
            var passwordHash = HashPassword(request.Password);
            var user = new User
            {
                Account = request.Account,
                Password = passwordHash
            };
            _repository.ApplyUser(user);

            user.Password = "*****";
            return user;
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public async Task<ApiRes<bool>> ValidateToken(string token)
        {
            var res = new ApiRes<bool>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // 根據需求可以設置為 true
                    ValidateAudience = false, // 根據需求可以設置為 true
                    ClockSkew = TimeSpan.Zero // 可選，設置為零以避免延遲
                }, out SecurityToken validatedToken);

                // 解析過期時間
                var jwtToken = validatedToken as JwtSecurityToken;
                var expiry = jwtToken?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

                if (expiry != null && DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry)) < DateTimeOffset.UtcNow)
                {
                    res.Result = false;
                    res.Data = false; // token 已過期
                    res.Msg = "Token 已過期";
                }
                else
                {
                    res.Result = true;
                    res.Data = true; // token 有效
                    res.Msg = "Token 驗證成功";
                }
            }
            catch (SecurityTokenExpiredException)
            {
                res.Result = false;
                res.Data = false; // token 已過期
                res.Msg = "Token 已過期";
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.Data = false; // token 無效
                res.Msg = "Token 驗證失敗: " + ex.Message;
            }

            return res;
        }
    }
}