using IKEA.DAL.Common.Enum;
using IKEA.DAL.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Data.Confgurations.Employees
{
    public class EmployeesConfgurations : IEntityTypeConfiguration<Employee>
    {
      
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E=>E.Name).HasColumnName("Name").HasColumnType("varchar(50)").IsRequired();
            builder.Property(E => E.Address).HasColumnName("Address").HasColumnType("varchar(100)");
            builder.Property(E=>E.Salary).HasColumnName("Salary").HasColumnType("decimal(8,2)");
            builder.Property(E=>E.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            #region Enum

            #region Gender

            builder.Property(E => E.Gender)
                .HasConversion
                (
                  (gender)=>gender.ToString(),
                  (gender)=>(Gender) Enum.Parse(typeof(Gender), gender)

                );

            #endregion

            #region EmployeeType
            builder.Property(E => E.EmployeeType)
                .HasConversion
                (
                  (employeeType) => employeeType.ToString(),
                  (employeeType) => (EmployeeType)Enum.Parse(typeof(EmployeeType), employeeType)
                );
            #endregion

            #endregion

            #region For Work Relationship

            builder.HasOne(E => E.Department)
                   .WithMany(D => D.Employees)
                   .HasForeignKey(E => E.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            #endregion
        }
    }

}
