using Humanizer;
using IKEA.BLL.Models.Departments;
using IKEA.BLL.Models.Employees;
using IKEA.BLL.Services.Departments;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using IKEA.PL.Models.Departments;
using IKEA.PL.Models.Empolyees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKEA.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Servics

        private readonly IEmployeeServics _employeeServics;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeServics employeeServics,
            IWebHostEnvironment webHostEnvironment,
            ILogger<EmployeeController> logger
       
            ) // As CLR for Creating Object From EmployeeServics Implicitly
        {
            _employeeServics = employeeServics;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        #endregion

        #region Index

        // BaseUrl/Employee/Index

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {

            var employees = await _employeeServics.GetEmployeesAsync(search);

            /// if (Request.IsAjaxRequest()) 
            ///if (!string.IsNullOrEmpty(search))
            ///{
            ///    return PartialView("_EmployeeList", employees);
            ///    return PartialView("partials/EployeeListPartial", employees);
            ///
            ///}


            // Check if the request is an AJAX request
            // This is a common way to check for AJAX requests in ASP.NET Core
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("partials/EployeeListPartial", employees);
            }


            return View(employees);

            /// View's Dictionary : pass From Controller [Action]
            ///
            /// To View [From This View => Partial View, LayOut]
            ///
            /// 1-ViewData : = is A Dictionary Type Property(Introduced In ASP.Net FreameWork )
            /// 3.5 => it Helps Us To Transfer Data From Controller [Action] To View  
            ///
            /// ViewData["Massage"] = "Hello View Data";
            ///
            /// 2-ViewBag : = is A Dynamic Type Property(Introduced In ASP.Net FreameWork 4.0 Based On dynamic Property)
            ///
            /// it Helps Us To Transfer Data From Controller [Action] To View
            ///
            /// ViewBag.Massage = "Hello View Bag";
            ///
            
        }

        #endregion

        #region Create

        // BaseUrl/Employee/Create

        #region Get

        [HttpGet]
        public async Task<IActionResult> Create([FromServices] IDepartmentService departmentService)
        {
             ViewData["Departments"] = await departmentService.GetAllDepartmentsAsync();

            return View();
        }

        #endregion

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedEmployeeToReturnDTO employee)
        {

            if (!ModelState.IsValid) // Server Side Validation
                return View(employee);

            var massge = string.Empty;

            try
            {
                var result = await _employeeServics.CreateEmployeeAsync(employee);
                if (result > 0)
                {
                   TempData["Massage"] = "Employee Is Created";

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                   TempData["Massage"] = "Error in Creating Employee Please Try Again";

                    massge = "Error in Creating Employee Please Try Again";
                    ModelState.AddModelError(string.Empty, massge);
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                // 1- Log the error

                _logger.LogError(ex, ex.Message);

                // 2- Show the error to the user

                if (_webHostEnvironment.IsDevelopment())
                {
                    massge = ex.Message;
                    return View(employee);
                }
                else
                {
                    massge = "Error in Creating Employee Please Try Again";
                    return View("There is a problem, please try again later. ", massge);
                }

            }

        }



        #endregion

        #endregion

        #region Update

        // BaseUrl/Employee/Edit/1

        #region Get

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, [FromServices] 
            IDepartmentService 
            departmentService,
          [FromServices] IWebHostEnvironment 
            webHostEnvironment)
        {

            if (id is null)
            {
                return BadRequest(); // 400
            }
            var employee = await _employeeServics.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound(); // 404
            }

       
            ViewData["Departments"] = await departmentService.GetAllDepartmentsAsync();
            

            return View(new EmployeeViewModel()
            {

                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                HiringDate = employee.HiringDate,
                Image = null,
                CurrentImage = employee.Image,


            });

        }

        #endregion


        #region Post
        [HttpPost] // POST
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeToReturnDTO employee)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(employee);

            var message = string.Empty;

            try
            {
                var updated = await _employeeServics.UpdateEmployeeAsync(employee) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "Employee is not Updated";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Set Message

                _logger.LogError(ex, ex.Message);

                if (_webHostEnvironment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "The Employee is not Created";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);

        }

        #endregion

        #region Another Way [Post]

        //public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employee)
        //{
        //    if (!ModelState.IsValid) // Server Side Validation
        //    {
        //        return View(employee);
        //    }

        //    var massage = string.Empty;
        //    try
        //    {

        //        #region Another Way

        //        //  var update = await _employeeServics.GetEmployeeByIdAsync(employee) > 0;


        //        // 1- Get the Employee from the database

        //        //var updatedEmployee = new UpdatedEmployeeToReturnDTO()
        //        //{
        //        //    Id = id,
        //        //    Name = employee.Name,
        //        //    Address = employee.Address,
        //        //    Email = employee.Email,
        //        //    PhoneNumber= employee.PhoneNumber,
        //        //    IsActive = employee.IsActive,
        //        //    EmployeeType = employee.EmployeeType,
        //        //    Age= employee.Age,
        //        //    Salary = employee.Salary,
        //        //    Gender = employee.Gender,
        //        //    HiringDate= employee.HiringDate,
        //        //    DepartmentId = employee.DepartmentId,
        //        //    Image = employee.Image,


        //        //}; 

        //        #endregion


        //        // 2- Update the Employee in the database
        //        var update = await _employeeServics.UpdateEmployeeAsync(employee) > 0;

        //        // 3- Check if the update was successful

        //        if (update)
        //        {
        //            TempData["Massage"] = " Employee Is Edited ";


        //            return RedirectToAction(nameof(Index));

        //            massage = "Employee Is Not Update ";
        //        }

        //        //else
        //        //{
        //        //   TempData["Massage"] = "Error in Editing Employee Please Try Again";

        //        //    // 4- If the update was not successful, show an error message

        //        //    massage = "Error in Updating Employee Please Try Again";
        //        //}

        //    }
        //    // 5- If there was an exception, log the error and show an error message
        //    catch (Exception ex)
        //    {
        //        // 1- Log the error
        //        _logger.LogError(ex, ex.Message);
        //        // 2- Show the error to the user
        //        massage = _environment.IsDevelopment() ? ex.Message :
        //            "Error in Updating Employee Please Try Again";
        //    }
        //    ModelState.AddModelError(string.Empty, massage);


        //    return View(employee);





        //}


        #endregion


        #endregion

        #region Details
        // BaseUrl/Employee/Details/1
        [HttpGet]
         public async Task<IActionResult> Details(int? id)
         {
            if(id is null)
            {
                return BadRequest(); // 400
            }

             var employee = await _employeeServics.GetEmployeeByIdAsync(id.Value);
             if (employee == null)
             {
                 return NotFound(); // 404
             }
             return View(employee);
         }
        #endregion

        #region Delete

        // BaseUrl/Employee/Delete/id

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var massage = string.Empty;
            try
            {
                var deleteted = await _employeeServics.DeleteEmployeeAsync(id);
                if (deleteted)
                {
                  //  TempData["Massage"] = " Employee Is Deleteted ";


                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                 //   TempData["Massage"] = "Error in Deleting Employee, Please Try Again";


                    massage = "Error in Deleting Employee Please Try Again";
                    ModelState.AddModelError(string.Empty, massage);
                    return View();
                }
            }
            catch (Exception ex)
            {


                // 1- Log the error
                _logger.LogError(ex, ex.Message);

                // 2- Show the error to the user
                // if (_webHostEnvironment.IsDevelopment())
                // {
                massage = _webHostEnvironment.IsDevelopment() ? ex.Message : "Error in Deleting Employee Please Try Again";

                // }
                // else
                // {
                // massage = "Error in Deleting Department Please Try Again";
                // return View("There is a problem, please try again later. ", massage);
                //}
            }
            return RedirectToAction(nameof(Index));


        }
        #endregion

        #endregion
    }
}
