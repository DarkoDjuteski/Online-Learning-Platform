using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearningPlatform.Infrastructure.Abstraction
{
    public interface IUnitOfWork<TContext>
    {
        IGenericRepository<TEntity> GetGenericRepository<TEntity>()
         where TEntity : class;
        Task SaveChangesAsync();
        IEnumerable<TEntity> ExecuteDataFromSqlCommand<TEntity>(string command)
            where TEntity : class;
    }
}
