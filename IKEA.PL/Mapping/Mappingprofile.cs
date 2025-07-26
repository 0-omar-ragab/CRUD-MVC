using AutoMapper;
using IKEA.BLL.Models.Departments;
using IKEA.BLL.Models.Employees;
using IKEA.PL.Models.Departments;
using IKEA.PL.Models.Empolyees;

namespace IKEA.PL.Mapping
{
    public class Mappingprofile:Profile
    {
        public Mappingprofile() 
        {

            #region Department

           CreateMap<DepartmentDetalisToReturnDTO, 
           DepartmentEditViewModel>().ReverseMap();

            CreateMap<DepartmentEditViewModel, UpdatedDepartmentDTO>();

            #endregion


            #region Employee

           // CreateMap<EmployeeDetalisToReturnDTO, EmployeeViewModel>();
           

        #endregion

        }


    }
}
