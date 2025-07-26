using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Models.Departments
{
   /// <summary>
           /// This Class is For
          /// Everything that Will be Displayed On 
         /// The Website And Has Nothing 
        /// To Do With The Database
       /// It Will Be Called In 
      /// this interface ( IDepartmentServics )
     /// Using GetDepartmentById() Method
    /// 
   /// </summary>
   


    public class DepartmentDetalisToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        [Display(Name = " Creation Date ")]
        public DateOnly CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModificationBy { get; set; }
        public DateTime LastModificationOn { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Employees Count")]
        public int EmployeesCount { get; set; }
    }
}
