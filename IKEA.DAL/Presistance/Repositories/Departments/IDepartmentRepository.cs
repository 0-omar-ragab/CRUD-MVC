using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Repositories.Departments
{



    /// <summary>
    /// The IDepartmentRepository interface defines 
    /// the contract for a repository that manages
    /// Department entities.
    /// It provides methods for CRUD 
    /// operations and querying the database.
    /// </summary>
    public interface IDepartmentRepository:IGenericRepository<Department>
    { 
        
    }
}
