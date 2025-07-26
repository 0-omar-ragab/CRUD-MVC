using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage =" Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } =null!;
    }
}
