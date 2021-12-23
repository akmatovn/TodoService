using Todo.BLL.Models;

namespace TodoApiDTO.Models
{
    public class ToDoItemResponseModel : ToDoItemRequestModel
    {
        public long Id { get; set; }
        public ToDoItemResponseModel() { }

        public ToDoItemResponseModel(ToDoItemBusinessModel model)
        {
            Id = model.Id;
            Name = model.Name;
            IsComplete = model.IsComplete;
            Secret = model.Secret;
        }

        public ToDoItemBusinessModel ToModel() =>
            new ToDoItemBusinessModel
            {
                Id = Id,
                Name = Name,
                IsComplete = IsComplete,
                Secret = Secret
            };

    }
}
