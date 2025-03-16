using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrightNight.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; } // 存儲加密後的密碼
    }

    public class AuthRequest
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AuthChangeRequest
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
}

    public class Auth
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime? ValidDate { get; set; }
    }
}