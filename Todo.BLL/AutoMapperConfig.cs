using AutoMapper;
using Todo.BLL.Models;
using Todo.DAL.Entities;

namespace Todo.BLL
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TodoItem, ToDoItemBusinessModel>();
            CreateMap<ToDoItemBusinessModel, TodoItem>();
        }
    }
}
