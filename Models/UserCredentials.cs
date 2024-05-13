using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    public class UserCredentials
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
