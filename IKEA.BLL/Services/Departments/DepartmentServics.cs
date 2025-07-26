using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.Departments
{
    
    public class DepartmentServics : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor
        public DepartmentServics(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }
        #endregion

        #region GetAllDepartments

        public async Task<IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync()
        {
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsQueryable()
                .Select(D => new DepartmentToReturnDTO
            {
                Id = D.Id,
                Name = D.Name,
                Code = D.Code,
                Description = D.Description,
                CreationDate = D.CreationDate,
                EmployeesCount = D.Employees.Count
            }).AsNoTracking().ToListAsync();
            return departments;
        }

        #endregion

        #region GetDepartmentById

        public async Task<DepartmentDetalisToReturnDTO?> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is { })
            {
                return new DepartmentDetalisToReturnDTO
                {

                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModificationBy = department.LastModificationBy,
                    LastModificationOn = department.LastModificationOn,
                    EmployeesCount = department.Employees.Count,
                };
            }
            return null;

        }

        #endregion

        #region CreateDepartment

        public async Task<int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO)
        {
           //CreatedDepartmentDTO departmentDTO1 = departmentDTO;
            var createddepartment = new Department
            {
                Name = departmentDTO.Name,
                Code = departmentDTO.Code,
                Description = departmentDTO.Description,
                CreationDate = departmentDTO.CreationDate,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow,
                // CreatedOn = DateTime.UtcNow,
            };
            _unitOfWork.DepartmentRepository.Add(createddepartment);
            return await _unitOfWork.CompleteAsync();
        }

        #endregion

        #region UpdateDepartment

        public async Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO departmentDTO)
        {
            var updatedDepartment = new Department
            {
                Id = departmentDTO.Id,
                Code = departmentDTO.Code,
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                CreationDate = departmentDTO.CreationDate,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow,
            };
            _unitOfWork.DepartmentRepository.Update(updatedDepartment);
            return await _unitOfWork.CompleteAsync();
        }

       




        //public int UpdateDepartment(UpdatedDepartmentDTO departmentDTO)
        //{
        //    var updatedDepartment = new Department
        //    {
        //        Id = departmentDTO.Id,
        //        Name = departmentDTO.Name,
        //        Code = departmentDTO.Code,
        //        Description = departmentDTO.Description,
        //        CreationDate = departmentDTO.CreationDate,
        //        LastModificationBy = 1,
        //        LastModificationOn = DateTime.UtcNow,
        //    };
        //    _unitOfWork.DepartmentRepository.Update(updatedDepartment);
        //    return _unitOfWork.Complete();
        //}

        #endregion

        #region DeleteDepartment

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var departmentRepository = _unitOfWork.DepartmentRepository;
            var department = await departmentRepository.GetByIdAsync(id);
            if (department is not null)
            {
                departmentRepository.Delete(department);
            }
            return await _unitOfWork.CompleteAsync() > 0;
        }




        #endregion

    }
}
