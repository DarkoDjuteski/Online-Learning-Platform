using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineLearningPlatform.Infrastructure.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearningPlatform.Infrastructure.Sql
{
    public class GenericRepository<TEntity, TContex> : IGenericRepository<TEntity> where TEntity : class where TContex : DbContext
    {
        protected DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(
            DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public object ReturnContext(object obj)
        {
            context.Entry(obj).State = EntityState.Detached;
            context.Entry(obj).State = EntityState.Added;
            return obj;
        }

        public virtual async Task<TEntity> GetByIdAsync(params object?[]? keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete == null) return false;
            return Delete(entityToDelete);

        }

        public virtual bool Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            return true;
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await dbSet.ToListAsync();
        }
        public virtual IEnumerable<TEntity> AllAsQueryble()
        {
            return dbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> UpsertAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            var existingEntity = await dbSet.SingleOrDefaultAsync(predicate);

            if (existingEntity != null)
            {
                var existingId = existingEntity.GetType().GetProperty("Id")?.GetValue(existingEntity);
                entity.GetType().GetProperty("Id")?.SetValue(entity, existingId);
                dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                await dbSet.AddAsync(entity);
            }

            return entity;
        }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IQueryable<TEntity> GetAsQueryable(
         Expression<Func<TEntity, bool>>? filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
         Expression<Func<TEntity, bool>>? filter = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        //public IEnumerable<TEntity> ExecuteDataFromSqlCommand(FormattableString command)
        //{
        //    return context.Set<TEntity>().FromSql(command);
        //}
    }
}
