using IKEA.BLL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.Employees
{
    public interface IEmployeeServics
    {
        Task<IEnumerable<EmployeeToReturnDTO>> GetEmployeesAsync(string search);   //(string search);
        Task<EmployeeDetalisToReturnDTO?> GetEmployeeByIdAsync(int id);
       Task<int> CreateEmployeeAsync(CreatedEmployeeToReturnDTO employeeDto);
       Task<int> UpdateEmployeeAsync(UpdatedEmployeeToReturnDTO employeeDto);
       Task<bool> DeleteEmployeeAsync(int id);
    }
}
