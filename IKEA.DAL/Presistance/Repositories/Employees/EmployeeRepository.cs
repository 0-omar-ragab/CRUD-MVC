using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories._Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Repositories.Employees
{
    public class EmployeeRepository :GenericRepository<Employee>, IEmployeeRepository
    {


        #region  Constructor Dependency Injection

        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            // Ask Clr for object From ApplicaonDbContext Implicitly
        }

        #endregion
    }
}
