using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Todo.DAL.Entities;
using Todo.DAL.GenericRepository;

namespace Todo.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;
        private bool _disposed;
        public DbContext Context => _context;

        public UnitOfWork(TodoContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();
            _disposed = false;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return _repositories.GetOrAdd(typeof(TEntity),
                x => new GenericRepository<TEntity>(_context)) as IGenericRepository<TEntity>;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
            }
        }

        public void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
