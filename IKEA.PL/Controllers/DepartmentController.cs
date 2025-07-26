using System.Linq.Expressions;
using IKEA.BLL.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IKEA.DAL.Models.Departments;
using IKEA.PL.Models.Departments;
using IKEA.BLL.Services.Departments;
using AutoMapper;
using System.Threading.Tasks;



namespace IKEA.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;


        #region Services
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment, IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }
        #endregion

        #region Index

        // BaseUrl/Department/Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            // View's Dictionary : pass From Controller [Action]

            // To View [From This View => Partial View, LayOut]

            // 1-ViewData : = is A Dictionary Type Property(Introduced In ASP.Net FreameWork )
            // 3.5 => it Helps Us To Transfer Data From Controller [Action] To View  

            ViewData["Massage"] = "Hello View Data";

            // 2-ViewBag : = is A Dynamic Type Property(Introduced In ASP.Net FreameWork 4.0 Based On dynamic Property)

            // it Helps Us To Transfer Data From Controller [Action] To View

            ViewBag.Massage = "Hello View Bag";
            var department = await _departmentService.GetAllDepartmentsAsync();
            return View(department);
        }

        #endregion

        #region Create

        // BaseUrl/Department/Create

        #region Get

        [HttpGet]
        public async Task<IActionResult> Create() 
        { 
            return View();
        }

        #endregion

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedDepartmentDTO departmentMV)
        {

            if (!ModelState.IsValid) // Server Side Validation
            return View(departmentMV);
            
            var message = string.Empty;

            try
            {
                var result = await _departmentService.CreateDepartmentAsync(departmentMV);

                // 3- TempData : is A Property of Type Dictionary Object
                // Introduced in ASP.Net FreameWork 3.5: Used For Transering the Data
                // Between 2 Requests

                if (result > 0)
                {
                    TempData["Massage"] = "Department Is Created";

                   return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["Massage"] = "Error in Creating Department Please Try Again";
                     message = "Error in Creating Department Please Try Again";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentMV);
                }
            }
            catch (Exception ex)
            {
                // 1- Log the error

                _logger.LogError(ex, ex.Message);

                // 2- Show the error to the user

                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentMV);
                }
                else
                {
                    message = "Error in Creating Department Please Try Again";
                    return View("There is a problem, please try again later. ", message);
                }

            }

        }



        #endregion

        #endregion

        #region Edit
        // BaseUrl/Department/Edit/id
        #region Get
          
        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) 
            {
                return BadRequest(); // 400
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null) 
            {
                return NotFound(); // 404
            }

            // Map the department to the view model
            var departmentVM2 = _mapper.Map<DepartmentDetalisToReturnDTO, 
                DepartmentEditViewModel>(department);

            return View(departmentVM2);
        }

        #endregion

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid) // Server Side Validation
            { 
                return View(departmentVM); 
            }

            var massage = string.Empty;
            try
            {
                // 1- Get the department from the database

                #region Another Way (Manual Mapping)


                ///var updatedDepartment = new UpdatedDepartmentDTO()
                ///{
                ///    Id = id,
                ///    Code = departmentVM.Code,
                ///    Name = departmentVM.Name,
                ///    Description = departmentVM.Description,
                ///    CreationDate = departmentVM.CreationDate 

                #endregion  

                 #region Mapping using AutoMapper

                var updatedDepartmentMap = _mapper.Map<UpdatedDepartmentDTO>(departmentVM);

                #endregion 

                // 2- Update the department in the database
                var update = await _departmentService.UpdateDepartmentAsync(updatedDepartmentMap) > 0;

                // 3- Check if the update was successful

                if (update)
                {
                    TempData["Massage"] = " Department Is Edited ";


                    return RedirectToAction(nameof(Index));
                }
                // 4- If the update was not successful, show an error message
                else
                {
                    TempData["Massage"] = "Error in Editing Department Please Try Again";


                    massage = "Error in Updating Department Please Try Again";
                }
                                 
            }
                 // 5- If there was an exception, log the error and show an error message
               catch (Exception ex)
            {
                // 1- Log the error
                _logger.LogError(ex, ex.Message);
                // 2- Show the error to the user
                massage = _environment.IsDevelopment() ? ex.Message : 
                    "Error in Updating Department Please Try Again";
            }
            ModelState.AddModelError(string.Empty, massage);
            return View(departmentVM);
        }


        #endregion

        #endregion

        #region Detalis

        // BaseUrl/Department/Detalis/id
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound(); // 404
            }
            return View(department);
        }


        #endregion

        #region Delete
        // BaseUrl/Department/Delete/id

      

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            
            var massage = string.Empty;
            try
            {
                var deleteted = await _departmentService.DeleteDepartmentAsync(id);
                if (deleteted)
                {
                    TempData["Massage"] = " Department Is Deleteted ";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Massage"] = "Error in Deleting Department, Please Try Again";

                    massage = "Error in Deleting Department, Please Try Again";
                    ModelState.AddModelError(string.Empty, massage);
                    return View();
                }
            }
            catch (Exception ex)
            {


                // 1- Log the error
                _logger.LogError(ex, ex.Message);

                // 2- Show the error to the user
               // if (_environment.IsDevelopment())
               // {
                    massage = _environment.IsDevelopment() ? ex.Message : "Error in Deleting Department Please Try Again";
                
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
