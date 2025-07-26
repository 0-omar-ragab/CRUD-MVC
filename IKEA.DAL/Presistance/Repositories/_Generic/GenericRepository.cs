using IKEA.DAL.Models;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {

        private readonly ApplicationDbContext _DbContext;

        #region  Constructor Dependency Injection
        public GenericRepository(ApplicationDbContext dbContext)
        {
            // Ask Clr for object From ApplicaonDbContext Implicitly
            _DbContext = dbContext;
        }

        #endregion


        #region Implementation interface IDepartmentRepository


        #region IEnumerable

        public async Task<IEnumerable<T>> GetAllAsync(bool WithNoTracking = true)
        {
            // WithNoTracking is a flag that indicates whether to track the entities returned by the query
            // If true, the entities are not tracked by the context, which can improve performance for read-only queries

            if (WithNoTracking)
            {
              return await _DbContext.Set<T>().Where(x=>x.IsDeleted).AsNoTracking().ToListAsync();
            }
            return await _DbContext.Set<T>().Where(x => x.IsDeleted).ToListAsync();
        }

        #endregion

        #region GetAllAsQueryable
        public IQueryable<T> GetAllAsQueryable()
        {
            // Return the DbSet as an IQueryable
            return _DbContext.Set<T>();
        }
        #endregion

        #region GetById

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _DbContext.Set<T>()
                .FindAsync(id);


            /// var T = _DbContext.T
            ///    .Local.FirstOrDefault(d => d.Id == id);
            /// return T;
        }

        #endregion

        #region Add

        public void Add(T entity)
        {
            // Add the entity to the context
            _DbContext.Set<T>().Add(entity);
        
        }

        #endregion

        #region Update

        public void Update(T entity)
        {
            // Attach the entity to the context
            _DbContext.Set<T>().Update(entity);

            //**********************************
            #region Another way

            // Attach the entity to the context
            // _DbContext.T.Attach(entity);
            // Mark the entity as modified
            // _DbContext.Entry(entity).State = EntityState.Modified;
            // Save changes to the database
            // return _DbContext.SaveChanges(); 

            #endregion
        }
        #endregion

        #region Delete

        ///  public int Delete(T entity)
        ///  {
        ///      // Check if the entity is already marked as deleted
        ///      entity.IsDeleted = true;
        ///
        ///      // Update the entity to the context
        ///      _DbContext.Set<T>().Update(entity);
        ///
        ///      // Save changes to the database
        ///      return _DbContext.SaveChanges();
        ///  }

        #region Delete

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
             
            _DbContext.Set<T>().Remove(entity); 
        }

        #endregion


        #endregion


        #endregion



    }


}
