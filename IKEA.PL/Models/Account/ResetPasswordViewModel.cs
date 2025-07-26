using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The Password Password Doesn't Match.")]
        public string NewConfirmPassword { get; set; } = null!;
    }
}
