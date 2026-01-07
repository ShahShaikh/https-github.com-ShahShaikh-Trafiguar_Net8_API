using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafiguraAssessment.Domain;
using TrafiguraAssessment.Domain.Entities;
using TrafiguraAssessment.Infrastructure.Interface;
using TrafiguraAssessment.Infrastructure.Repository;

namespace TrafiguraAssessment.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AssessmentDBContext _context;
        private IDbContextTransaction transaction = default!;
        public bool _disposed;
        private string _errorMessage = string.Empty;

        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(AssessmentDBContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.TryGetValue(type, out var repo))
            {
                var genericRepo = new GenericRepository<T>(_context);
                _repositories[type] = genericRepo;
                return genericRepo;
            }
            return (IGenericRepository<T>)repo;
        }
        public void CreateTransaction()
        {
            transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();
            _disposed = true;
        }

        public void Rollback()
        {
            transaction.Rollback();
            transaction.Dispose();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validaionErrors in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Error: {0}", validationErrors.ValidationErrors);
                throw new Exception(_errorMessage, dbEx);
            }

        }
        public void SaveAsync()
        {
            try
            {
                _context.SaveChangesAsync();
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
