using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Data.Confgurations.Deparments
{
    public class DepartmentsConfgurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("varchar(50)").IsRequired();
            builder.Property(D => D.Code).HasColumnType("varchar(50)").IsRequired();
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(D => D.LastModificationOn).HasComputedColumnSql("GETDATE()");

            #region For Work Relationship

            builder.HasMany(D => D.Employees)
                      .WithOne(E => E.Department)
                      .HasForeignKey(E => E.DepartmentId)
                      .OnDelete(DeleteBehavior.SetNull); 

            #endregion
        }
    }
}

