using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrafiguraAssessment.Domain.Entities;
using TrafiguraAssessment.Infrastructure.Interface;

namespace TrafiguraAssessment.Infrastructure.Repository
{
    public class GenericRepository<T> :IGenericRepository<T>, IDisposable where T : class
    {
        private bool _disposed;
        private AssessmentDBContext _context;
        protected readonly DbSet<T> dbset;
        private string _errorMessage = string.Empty;

        public GenericRepository(AssessmentDBContext context)
        {
            _context = context;
            dbset = context.Set<T>();
        }
        public IQueryable<T> Query()
        {
            // Simply return IQueryable so LINQ can be applied
            return dbset.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbset.AnyAsync(predicate);
        }
        public async Task<T> FindFirst(Expression<Func<T, bool>> predicate)
        {
            return await dbset.Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbset.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbset.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                await _context.Set<T>().AddRangeAsync(entities);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validaionErrors in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Error: {0}", validationErrors.ValidationErrors);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                dbset.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validaionErrors in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Error: {0}", validationErrors.ValidationErrors);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
        public void Update(T entity)
        {
            dbset.Update(entity);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                _context.Set<T>().UpdateRange(entities);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validaionErrors in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Error: {0}", validationErrors.ValidationErrors);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));
                _context.Set<T>().RemoveRange(entities);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validaionErrors in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Error: {0}", validationErrors.ValidationErrors);
                throw new Exception(_errorMessage, dbEx);
            }
        }
    }
}
