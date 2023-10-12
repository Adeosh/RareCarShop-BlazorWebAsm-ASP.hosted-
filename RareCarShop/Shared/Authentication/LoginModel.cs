using System;
using System.ComponentModel.DataAnnotations;

namespace RareCarShop.Shared.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
