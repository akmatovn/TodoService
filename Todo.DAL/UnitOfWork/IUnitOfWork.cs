using System;
using System.Threading.Tasks;
using Todo.DAL.GenericRepository;

namespace Todo.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task SaveChangesAsync();
        void Dispose(bool disposing);
    }
}
