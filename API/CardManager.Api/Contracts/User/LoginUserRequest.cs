using System.ComponentModel.DataAnnotations;

namespace CardManager.Api.Contracts.User
{
    public class LoginUserRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}