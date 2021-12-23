using System.ComponentModel.DataAnnotations;
using Todo.BLL.Models;

namespace TodoApiDTO.Models
{
    public class ToDoItemRequestModel
    {
        [Required(ErrorMessage = "{0} обязательное поле для заполнения!", AllowEmptyStrings = false)]
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public string Secret { get; set; }

        public ToDoItemBusinessModel ToModel() =>
            new ToDoItemBusinessModel
            {
                Name = Name,
                IsComplete = IsComplete,
                Secret = Secret
            };
    }
}
