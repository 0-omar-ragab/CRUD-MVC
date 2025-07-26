using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories._Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Repositories.Departments
{


    /// <summary>
    /// DbContext is a class that 
    /// represents a session 
    /// with the database, allowing 
    /// us to query and save data
    /// </summary>

    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        #region  Constructor Dependency Injection
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            // Ask Clr for object From ApplicaonDbContext Implicitly
        }

        #endregion
    }
}
