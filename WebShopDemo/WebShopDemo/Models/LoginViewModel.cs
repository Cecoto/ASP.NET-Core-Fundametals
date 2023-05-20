﻿using System.ComponentModel.DataAnnotations;

namespace WebShopDemo.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        
        public string? ReturnUrl { get; set; }         
    }
}
