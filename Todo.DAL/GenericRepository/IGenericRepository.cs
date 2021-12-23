using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Todo.DAL.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> All { get; }
        Task<IEnumerable<T>> AllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        void Remove(T entity);
    }
}
