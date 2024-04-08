using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineLearningPlatform.Infrastructure.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearningPlatform.Infrastructure.Sql
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
    {
        private TContext _context;
        private readonly ILogger _logger;


        public UnitOfWork(TContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> GetGenericRepository<TEntity>()
         where TEntity : class
        {
            return new GenericRepository<TEntity, TContext>(_context);
        }

        public IEnumerable<TEntity> ExecuteDataFromSqlCommand<TEntity>(string command)
         where TEntity : class
        {
            return _context.Set<TEntity>().FromSqlRaw(command);
        }

    }
}
