using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace Common.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Email { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        public string Password { get; set; }

        public LoginModel()
        {

        }
        public LoginModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}