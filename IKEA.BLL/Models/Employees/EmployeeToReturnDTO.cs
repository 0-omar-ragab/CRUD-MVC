using IKEA.DAL.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Models.Employees
{
    public class EmployeeToReturnDTO
    {
        public int Id { get; set; }

        // public bool IsDeleted { get; set; }
        //public int CreatedBy { get; set; }
        //public DateTime CreatedOn { get; set; }
        // public int LastModificationBy { get; set; }
        //public DateTime LastModificationOn { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        // public string Code { get; set; } = null!;

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        public string Gender { get; set; } = null!;
        public string EmployeeType { get; set; } = null!;

        public string Department { get; set; } = null!;
        public string? Image { get; set; }

        //public Gender Gender { get; set; } 
        //public EmployeeType EmployeeType { get; set; }

        /// [Display(Name = "Date Of Creation")]
        /// public DateOnly CreationDate { get; set; }
    }
} 
