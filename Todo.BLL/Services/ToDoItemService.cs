using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Todo.BLL.Exceptions;
using Todo.BLL.Interfaces;
using Todo.BLL.Models;
using Todo.DAL.Entities;
using Todo.DAL.GenericRepository;
using Todo.DAL.UnitOfWork;

namespace Todo.BLL.Services
{
    public class ToDoItemService : BaseService, IToDoItemService
    {
        private readonly IGenericRepository<TodoItem> _toDoItemRepo;
        private readonly IMapper _mapper;
        public ToDoItemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _toDoItemRepo = UnitOfWork.GetRepository<TodoItem>();
            _mapper = mapper;
        }

        public async Task<IEnumerable<ToDoItemBusinessModel>> TodoItemsAsync()
        {
            try
            {
                var entities = await _toDoItemRepo.AllAsync();
                return _mapper.Map<IEnumerable<ToDoItemBusinessModel>>(entities);
            }
            catch (SqlException ex)
            {
                throw new CustomException(-3, $"Во время обращения к базе данных произошла ошибка! Service '{nameof(TodoItemsAsync)}'.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException(-4, $"Во время обработки данных произошла ошибка! Service '{nameof(TodoItemsAsync)}'.", ex);
            }
        }

        public async Task<ToDoItemBusinessModel> AddToDoItemAsync(ToDoItemBusinessModel model)
        {
            try
            {
                var res = await _toDoItemRepo.AddAsync(_mapper.Map<TodoItem>(model));
                await UnitOfWork.SaveChangesAsync();
                return _mapper.Map<ToDoItemBusinessModel>(res);
            }
            catch (SqlException ex)
            {
                throw new CustomException(-5, $"Во время сохранения данных произошла ошибка! Service '{nameof(AddToDoItemAsync)}'.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException(-6, $"Во время обработки данных произошла ошибка! Service '{nameof(AddToDoItemAsync)}'.", ex);
            }
        }

        public async Task DeleteToDoItemAsync(long id)
        {
            try
            {
                _toDoItemRepo.Remove(await _toDoItemRepo.FirstOrDefaultAsync(x => x.Id == id));
                await UnitOfWork.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new CustomException(-7, $"Во время удаления данных произошла ошибка! Service '{ nameof(DeleteToDoItemAsync) }'.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException(-8, $"Во время обработки данных произошла ошибка! Service '{nameof(DeleteToDoItemAsync)}'.", ex);
            }
        }

        public async Task UpdateToDoItemAsync(ToDoItemBusinessModel model)
        {
            try
            {
                var todo = _mapper.Map<TodoItem>(model);
                _toDoItemRepo.Update(todo);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new CustomException(-9, $"Во время обновления данных произошла ошибка! Service '{nameof(UpdateToDoItemAsync)}'.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException(-10, $"Во время обработки данных произошла ошибка! Service '{nameof(UpdateToDoItemAsync)}'.", ex);
            }
        }

        public async Task<ToDoItemBusinessModel> ToDoItemByIdAsync(long id)
        {
            try
            {
                return _mapper.Map<ToDoItemBusinessModel>(await _toDoItemRepo.FirstOrDefaultAsync(x => x.Id == id));
            }
            catch (SqlException ex)
            {
                throw new CustomException(-11, $"Во время обращения к базе данных произошла ошибка! Service '{nameof(ToDoItemByIdAsync)}'.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException(-12, $"Во время обработки данных произошла ошибка! Service '{nameof(ToDoItemByIdAsync)}'.", ex);
            }
        }
    }
}
