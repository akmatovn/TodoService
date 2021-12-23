using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Todo.DAL.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _entities;

        private DbSet<TEntity> EntityDbSet => _entities.Set<TEntity>();

        public GenericRepository(DbContext context)
        {
            _entities = context;
        }

        public IQueryable<TEntity> All => EntityDbSet;

        public async Task<IEnumerable<TEntity>> AllAsync() => await EntityDbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await EntityDbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await EntityDbSet.AddAsync(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await EntityDbSet.FirstOrDefaultAsync(predicate);
        }

        public void Remove(TEntity entity)
        {
            EntityDbSet.Remove(entity);
        }
    }
}
