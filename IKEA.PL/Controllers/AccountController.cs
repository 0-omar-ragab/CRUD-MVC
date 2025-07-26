using Humanizer;
using IKEA.BLL.Common.Services.EmailSetting;
using IKEA.DAL.Entities.Identity;
using IKEA.DAL.Entities.SMS;
using IKEA.PL.Helpers;
using IKEA.PL.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IKEA.PL.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettting _emailSettting;
        private readonly ISMSService _sMSService;


        #region Services

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmailSettting emailSettting,
            ISMSService sMSService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettting = emailSettting;
            _sMSService = sMSService;
            // Initialize any services or dependencies here if needed
        }


        #endregion

        #region Register

        #region Get

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View();
        }

        #endregion

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check if The User already Exists
            var existingUser = await _userManager.FindByNameAsync(signUpViewModel.UserName);

            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(SignUpViewModel.UserName),
                    "This User Name is already Taken. ");
                return View(signUpViewModel);
            }

            var User = new ApplicationUser()
            {
                FName = signUpViewModel.FristName,
                LName = signUpViewModel.LastName,
                UserName = signUpViewModel.UserName,
                Email = signUpViewModel.Email,
                PhoneNumber = signUpViewModel.PhoneNumber,
                IsAgree = signUpViewModel.IsAgree

            };

            // Create the user
            var result = await _userManager.CreateAsync(User, signUpViewModel.Password);
            if (result.Succeeded)
            {
                // Sign in the user
                return RedirectToAction(nameof(SignIn));
            }
            // If there are errors, add them to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(signUpViewModel);
        }


        #endregion

        #endregion

        #region Login

        #region Get
        // password = Omar@hmed50
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            return View();
        }

        #endregion

        #region Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest();

            }
            var User = await _userManager.
               FindByEmailAsync
               (signInViewModel.Email);

            if (User is { })
            {
                var flag = await _userManager.
                    CheckPasswordAsync
                    (User, signInViewModel.Password);
                if (flag)
                {
                    var result = await _signInManager
                        .PasswordSignInAsync(User,
                        signInViewModel.Password,
                        signInViewModel.RememberMe, true
                        );
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty,
                            "You're not allowed to sign in. Please contact support.");
                    }

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty,
                            " Your Account Is Locked Please contact support and Try Again Later. ");
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }

                }

            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt. ");

            return View(signInViewModel);

        }


        #endregion

        #endregion

        #region LogOut

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            //return RedirectToAction(nameof(SignIn));
            return RedirectToAction(nameof(SignIn), "Account", new { returnUrl = "/" });
        }

        #endregion


        #region Chose Reset By PhoneNumber Or Email

        #region Get

        [HttpGet]

        public IActionResult ChoseResetByPhoneNumberOrEmail()
        {
            return View();
        }

        #endregion

        #region Pos

        [HttpPost]

        public async Task<IActionResult> ChoseResetByPhoneNumberOrEmail(string option)
        {
            if (option == "Email")
            {
                return RedirectToAction(nameof(ForgetPassword));
            }
            else if (option == "Phone")
            {
                return RedirectToAction(nameof(SendSms));
            }

            ModelState.AddModelError(string.Empty, "Invalid option selected.");
            return View();
        }

        #endregion


        #endregion


        #region Forgot Password

        #region Get

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {
            //return RedirectToAction(nameof(ChoseResetByPhoneNumberOrEmail));

            return View();
        }

        #endregion

        #region Post


        [HttpPost]
        public async Task<IActionResult> SendRessetPasswordUrl(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                    // https://localhost:7176/Account/oa242895@gmail.com

                    var url = Url.Action("ResetPassword", "Account",
                        new
                        {
                            email = forgetPasswordViewModel.Email,
                            token = token
                        }, Request.Scheme);

                    var email = new Email()
                    {

                        To = forgetPasswordViewModel.Email,
                        Subject = "Reset Your Password",
                        Body = url


                    };
                    // Send Email 

                    _emailSettting.SendEmail(email);

                    return RedirectToAction("CheckYourInbox");
                }
                ModelState.AddModelError(string.Empty, " Invalid Operation, Please Try Again Later ");
            }
            return View("ForgetPassword");

        }


        #endregion

        #endregion

        #region SendSms

        #region Get

        [HttpGet]
        public IActionResult SendSms()
        {
            return View();
        }

        #endregion

        #region Post


        [HttpPost]
        public async Task<IActionResult> SendSms(ForgetPasswordByPhoneViewModel forgetPasswordByPhoneViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == forgetPasswordByPhoneViewModel.PhoneNumber);
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                if (user is not null)
                {
                    var passwordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, Request.Scheme);

                    var sms = new SmsMessage()
                    {
                        PhoneNumber = user.PhoneNumber,
                        Body = $"To reset your password, click the following link ,):{passwordUrl}"
                    };
                    _sMSService.SendSms(sms);
                    return RedirectToAction("CheckYourInbox");
                    //return Redirect(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "There is not account with this Phone");
            }
            return View(forgetPasswordByPhoneViewModel);
        }


        #endregion


        #endregion


        #region Check Your Inbox

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }


        #endregion

        #region Reset Password

        #region Get

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, 
                        resetPasswordViewModel.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
                
            }
            ModelState.AddModelError(string.Empty, " Invalid Operation, Please Try Again Later ");

            return View(resetPasswordViewModel);
        }

        #endregion

        #endregion



    }
}
