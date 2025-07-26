using IKEA.DAL.Entities.Identity;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {

        #region ApplicaonDbContext

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          
         // modelBuilder.Entity<Department>().ToTable("Departments");
         // modelBuilder.Entity<Models.Employees.Employee>().ToTable("Employees");
        }
        #endregion

        #region Dbset

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        #endregion
    }
}
