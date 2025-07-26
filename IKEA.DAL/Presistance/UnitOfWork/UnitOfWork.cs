using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork    
    {
        private readonly ApplicationDbContext _dbContext;

        #region EmployeeRepository

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                return new EmployeeRepository(_dbContext);
            }
        }

        #endregion

        #region DepartmentRepository

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return new DepartmentRepository(_dbContext);
            }
        }

        #endregion

        #region ApplicationDbContext

        public UnitOfWork(ApplicationDbContext dbContext)

        {

            _dbContext = dbContext;
        }

        #endregion

        #region CompleteAsync

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region DisposeAsync


        public async ValueTask DisposeAsync()
        {
           await _dbContext.DisposeAsync();
        }

        //public async ValueTask DisposeAsync()
        //{
        //    await _dbContext.DisposeAsync();
        //} 

        #endregion
    }
}
