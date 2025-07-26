using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.Departments
{
      /// <summary>
     /// This interface is For
    /// Everything that Will be Displayed On
   ///  The Website And Has Nothing
  ///   To Do With The Database
 /// </summary>


    public interface IDepartmentService
    {
        
        Task<IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync();

        Task<DepartmentDetalisToReturnDTO?> GetDepartmentByIdAsync(int id);

        Task<int> CreateDepartmentAsync(CreatedDepartmentDTO department);

        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO department);

        Task<bool> DeleteDepartmentAsync(int id);
    }
}
