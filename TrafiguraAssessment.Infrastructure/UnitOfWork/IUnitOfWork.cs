using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafiguraAssessment.Infrastructure.Interface;

namespace TrafiguraAssessment.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        void CreateTransaction();
        void Commit();
        void Dispose();
        void Rollback();
        void Save();
        void SaveAsync();
    }
}
