using IKEA.BLL.Common.Services;
using IKEA.BLL.Common.Services.EmailSetting;
using IKEA.BLL.Services.Departments;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Entities.Identity;
using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.UnitOfWork;
using IKEA.PL.Helpers;
using IKEA.PL.Mapping;
using IKEA.PL.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IKEA.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           

            #region Configure

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region ConnectionString Data Base


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {


                options.UseLazyLoadingProxies()
                      
                // UseSqlServer is an extension method provided by Microsoft.EntityFrameworkCore.SqlServer
                // It configures the context to use SQL Server with the specified connection string.
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            #endregion

            #region Another way to ConnectionString But Old Way


            /// builder.Services.AddScoped<DbContextOptions<ApplicaonDbContext>>(provider =>
            ///    {
            ///        var optionsBuilder = new DbContextOptionsBuilder<ApplicaonDbContext>();
            ///
            ///        optionsBuilder.UseSqlServer("Server=.;Database=ProjectMVC_IKEA;Trusted_Connection=True;trustservercertificate=true");
            ///
            ///        var options = optionsBuilder.Options;
            ///
            ///        return options;
            ///
            ///
            ///        /** ** Another way ** **/
            ///        //    ^
            ///        //    |
            ///        // return optionsBuilder.Options;
            ///    }); 

            #endregion

            #region AddScopedDepartment And Employee

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IEmployeeServics, EmployeeServics>();

            builder.Services.AddScoped<IDepartmentService, DepartmentServics>();

            builder.Services.AddTransient<IAttachment, Attachment>();

            builder.Services.AddScoped<IEmailSettting, EmailSettting>();

            // builder.Services.Configure<MilSettings>(builder.Configuration.GetSection("EmailSettings"));

            //builder.Services.Configure<MilSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddScoped<ISMSService, SmSService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((Options =>
            {
                Options.Password.RequiredLength = 5; // Minimum length of password
                Options.Password.RequireLowercase = true; // Require at least one lowercase letter
                Options.Password.RequireUppercase = true; // Require at least one uppercase letter
                Options.Password.RequireDigit = true; // Require at least one digit
                Options.Password.RequireNonAlphanumeric = false; // Require at least one non-alphanumeric character
                Options.Lockout.AllowedForNewUsers = true; // Allow lockout for new users
                Options.Lockout.MaxFailedAccessAttempts = 5; // Maximum number of failed access attempts before lockout
                Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Default lockout time span
            }))
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";

            });

            

            #endregion

            #region AddAutoMapper

            builder.Services.AddAutoMapper(M=>M.AddProfile (new Mappingprofile()));

            #endregion 

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();


            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
