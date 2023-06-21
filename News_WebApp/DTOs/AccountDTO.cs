using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_WebApp_API.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must have Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.")]
        public string Password { get; set; }
    }

    public class RegisterDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LasttName { get; set; }
    }

    public class UserDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }


}
