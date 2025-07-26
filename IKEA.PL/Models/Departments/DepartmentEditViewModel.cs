using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Departments
{
    public class DepartmentEditViewModel
    {

        [Required(ErrorMessage = "Code is Required, Please Enter Your Code.   ")]
        public int id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly CreationDate { get; set; }
        public int EmployeesCount { get; set; }
    }
}
