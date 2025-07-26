using NHibernate.Type;
using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class SignUpViewModel
    {
        [Display(Name ="Frist Name")]
        public string FristName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        public string  UserName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = " Confirmation Password Doesn't match With Password.")]
        public string ConfirmPassword { get; set; } = null!;
        public bool IsAgree { get; set; }

    }
}
