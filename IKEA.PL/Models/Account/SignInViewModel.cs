using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class SignInViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
