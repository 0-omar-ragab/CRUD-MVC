using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ForgetPasswordByPhoneViewModel
    {
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format.")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
