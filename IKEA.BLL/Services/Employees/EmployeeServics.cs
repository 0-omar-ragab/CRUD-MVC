using FuzzySharp;
using IKEA.BLL.Common.Services;
using IKEA.BLL.Models.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace IKEA.BLL.Services.Employees
{
    public class EmployeeServics : IEmployeeServics
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachment _attachment;
        private readonly IHostingEnvironment _HostingEnvironment;

        #region Constructor
        public EmployeeServics(IUnitOfWork unitOfWork, IAttachment attachment, IHostingEnvironment HostingEnvironment)
        {
            // Ask Clr for Creating object From UnitOfWork Implicitly

            _unitOfWork = unitOfWork;
           _attachment = attachment;
            _HostingEnvironment = HostingEnvironment;
        }
        #endregion

        #region GetAllEmployees

        public async Task<IEnumerable<EmployeeToReturnDTO>> GetEmployeesAsync(string search)
        {
            return await _unitOfWork.EmployeeRepository.GetAllAsQueryable()
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) 
                || E.Name.ToLower().Contains(search.ToLower())))

                // Include the Department navigation property
                .Include(E => E.Department) 
                .Select
                (employeeDto => new EmployeeToReturnDTO
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                Gender = employeeDto.Gender.ToString(),
                EmployeeType =employeeDto.EmployeeType.ToString(),
                Department = employeeDto.Department.Name 
            }).ToListAsync();


            #region Another way From article on LinkedIn

        //    var query = _unitOfWork.EmployeeRepository
        //          .GetAllAsQueryable()
        //          .Where(E => !E.IsDeleted)
        //          .Include(E => E.Department);

        //    var employees = query.ToList();

        //    // If the user types search
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.ToLower();

        //        if (search.Length < 3)
        //        {
        //            // Use Contains
        //            employees = employees
        //                .Where(e => e.Name != null && e.Name.ToLower().Contains(search))
        //                .ToList();
        //        }
        //        else
        //        {
        //            // Use Fuzzy
        //            employees = employees
        //                .Where(e => e.Name != null && Fuzz.Ratio(e.Name.ToLower(), search) >= 80)
        //                .ToList();
        //        }
        //    }

        //    return employees.Select(employeeDto => new EmployeeToReturnDTO
        //    {
        //        Id = employeeDto.Id,
        //        Name = employeeDto.Name,
        //        Age = employeeDto.Age,
        //        IsActive = employeeDto.IsActive,
        //        Salary = employeeDto.Salary,
        //        Email = employeeDto.Email,
        //        Gender = employeeDto.Gender.ToString(),
        //        EmployeeType = employeeDto.EmployeeType.ToString(),
        //        Department = employeeDto.Department.Name
        //    }).ToList();
        //
        } 

        #endregion


        #endregion


        #region GetEmployeeById
        public async Task<EmployeeDetalisToReturnDTO?> GetEmployeeByIdAsync(int id)
        {
       

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee is { })
                return new EmployeeDetalisToReturnDTO()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    Salary = employee.Salary,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,
                    // CreatedOn = employee.CreatedOn.DateTime.UtcNow,
                    //LastModifiedOn =employee.LastModificationOn,
                    LastModifiedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    Department = employee?.Department?.Name,
                    Image = employee?.Image,

                }; 
            return null;
        }


        #endregion


        #region CreateEmployee
        public async Task<int> CreateEmployeeAsync(CreatedEmployeeToReturnDTO employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                Salary = employeeDto.Salary,
                IsActive = employeeDto.IsActive,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1, // This should be set to the current user's ID
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };

            // If the employee has an attachment, handle it
            if (employeeDto.Image is { })
            {
                // Save the image and get the path
                var imagePath =  _attachment.UploadFile(employeeDto.Image, "Images");
                employee.Image =  imagePath;
            }
          _unitOfWork.EmployeeRepository.Add(employee);
            return await _unitOfWork.CompleteAsync();
        }

        #endregion


        #region UpdateEmployee

        #region UpdateEmployee

        public async Task<int> UpdateEmployeeAsync(UpdatedEmployeeToReturnDTO employeeDto)
        {
            // 1. Get employee from DB
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeDto.Id);
            if (employee == null)
                throw new Exception("Employee not found");
             
            // 2. Update fields
            employee.Name = employeeDto.Name;
            employee.Age = employeeDto.Age;
            employee.Address = employeeDto.Address;
            employee.Salary = employeeDto.Salary ?? 0m;
            employee.IsActive = employeeDto.IsActive;
            employee.Email = employeeDto.Email;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.HiringDate = employeeDto.HiringDate;
            employee.Gender = employeeDto.Gender;
            employee.EmployeeType = employeeDto.EmployeeType;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.LastModificationBy = 1;
            employee.LastModificationOn = DateTime.UtcNow;

            // 3. Handle new image
            if (employeeDto.Image is not null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(employee.Image))
                {
                    var oldImagePath = Path.Combine(_HostingEnvironment.WebRootPath, "Files", "Images", "Images", employee.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                // Upload new image
                var imagePath = _attachment.UploadFile(employeeDto.Image, "Images");
                employee.Image = imagePath;
            }

            // 4. Save changes
            _unitOfWork.EmployeeRepository.Update(employee);
            return await _unitOfWork.CompleteAsync();
        }

        #endregion



        //public int UpdateEmployee(UpdatedEmployeeToReturnDTO employeeDto)
        //{
        //    var employee = new Employee()
        //    {
        //        Id = employeeDto.Id,
        //        Name = employeeDto.Name,
        //        Age = employeeDto.Age,
        //        Address = employeeDto.Address,
        //        Salary = employeeDto.Salary ?? 0m,
        //        IsActive = employeeDto.IsActive,
        //        Email = employeeDto.Email,
        //        PhoneNumber = employeeDto.PhoneNumber,
        //        HiringDate = employeeDto.HiringDate,
        //        Gender = employeeDto.Gender,
        //        EmployeeType = employeeDto.EmployeeType,
        //        DepartmentId = employeeDto.DepartmentId,
        //        CreatedBy = 1, // This should be set to the current user's ID
        //        LastModificationBy = 1,
        //        LastModificationOn = DateTime.UtcNow
        //    };

        //    // If the employee has an attachment, handle it
        //    if (employeeDto.Image is { })
        //    {

        //        if (!string.IsNullOrEmpty(employee.Image))
        //        {
        //            var oldImagePath = Path.Combine(_HostingEnvironment.WebRootPath, "Files", "Images", "Images", employee.Image);
        //            if (File.Exists(oldImagePath))
        //            {
        //                File.Delete(oldImagePath);
        //            }
        //        }
        //        // Save the image and get the path
        //        var imagePath = _attachment.UploadFile(employeeDto.Image, "Images");
        //        employee.Image = imagePath;
        //    }

        //    _unitOfWork.EmployeeRepository.Update(employee);
        //    return _unitOfWork.Complete();
        //}

        #endregion


        #region DeleteEmployee



        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employeeRepository = _unitOfWork.EmployeeRepository;

            var employee = await employeeRepository.GetByIdAsync(id);
            if (employee is { })
            {
                if (!string.IsNullOrEmpty(employee.Image))
                {
                    var fullImagePath = Path.Combine(_HostingEnvironment.WebRootPath, "Files", "Images", "Images", employee.Image);
                    if (File.Exists(fullImagePath))
                    {
                        File.Delete(fullImagePath);
                    }
                }
                employeeRepository.Delete(employee);
            }
            
            return await _unitOfWork.CompleteAsync() > 0;
        }


        #region AnotherWay but This Way It's not Deleted In DataBase

        ///public bool DeleteEmployee(int id)
        ///{
        ///    var employee = _employeeRepository.GetById(id);
        ///    if (employee is { })
        ///    {
        ///        return _employeeRepository.Delete(employee) > 0;
        ///    }
        ///    return false;
        ///}


        #endregion



        #endregion





    }

}
