using System;
using System.ComponentModel.DataAnnotations;

namespace RareCarShop.Shared.Authentication
{
    public class UserChangePass
    {
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
