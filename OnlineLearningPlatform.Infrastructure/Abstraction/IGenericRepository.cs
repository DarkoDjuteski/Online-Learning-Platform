using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearningPlatform.Infrastructure.Abstraction
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> AllAsync();
        IEnumerable<TEntity> AllAsQueryble();
        Task<TEntity> GetByIdAsync(params object?[]? keyValuess);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteAsync(int id);
        bool Delete(TEntity entityToDelete);
        Task<TEntity> UpsertAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        IQueryable<TEntity> GetAsQueryable(
         Expression<Func<TEntity, bool>>? filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        //IEnumerable<TEntity> ExecuteDataFromSqlCommand(FormattableString command);
    }
}
