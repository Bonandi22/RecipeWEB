using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "The Email field is not a valid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        public bool IsBloked { get; set; }
        public bool IsAdmin { get; set; }

        public User() { }
        public User(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }
    }

}
