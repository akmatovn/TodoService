using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.BLL.Models;

namespace Todo.BLL.Interfaces
{
    public interface IToDoItemService
    {
        Task<IEnumerable<ToDoItemBusinessModel>> TodoItemsAsync();
        Task<ToDoItemBusinessModel> AddToDoItemAsync(ToDoItemBusinessModel model);
        Task UpdateToDoItemAsync(ToDoItemBusinessModel model);
        Task DeleteToDoItemAsync(long id);
        Task<ToDoItemBusinessModel> ToDoItemByIdAsync(long id);
    }
}
