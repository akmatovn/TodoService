using Todo.DAL.UnitOfWork;

namespace Todo.BLL.Services
{
    public class BaseService
    {
        protected IUnitOfWork UnitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
