using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.BLL.Exceptions;
using Todo.BLL.Interfaces;
using TodoApiDTO.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]"), Produces("application/json")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(IToDoItemService toDoItemService, ILogger<TodoItemsController> logger)
        {
            _toDoItemService = toDoItemService;
            _logger = logger;
        }

        [HttpGet("Items")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ToDoItemResponseModel>>), 200)]
        [ProducesErrorResponseType(typeof(BaseResponse<string>))]
        public async Task<ActionResult<IEnumerable<ToDoItemResponseModel>>> GetTodoItems()
        {
            try
            {
                var res = await _toDoItemService.TodoItemsAsync();
                var data = res.Select(x => new ToDoItemResponseModel(x));
                return Ok(new BaseResponse<IEnumerable<ToDoItemResponseModel>> { Code = 0, Message = "Ok", Data = data });
            }
            catch (CustomException ex)
            {
                var innerEx = ex.InnerException == null ? "null" : ex.InnerException.Message;
                _logger.LogError(ex, innerEx);
                return BadRequest(new BaseResponse<string> { Code = ex.Code, Message = ex.Message, Data = innerEx });
            }
        }

        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(BaseResponse<ToDoItemResponseModel>), 200)]
        [ProducesErrorResponseType(typeof(BaseResponse<string>))]
        public async Task<ActionResult<ToDoItemResponseModel>> GetTodoItem(long id)
        {
            try
            {
                var res = await _toDoItemService.ToDoItemByIdAsync(id);
                if (res == null)
                    return Ok(new BaseResponse<ToDoItemResponseModel> { Code = 0, Message = $"Задача с номером: '{id}' отсутствует!", Data = null });

                return Ok(new BaseResponse<ToDoItemResponseModel> { Code = 0, Message = "Ok", Data = new ToDoItemResponseModel(res) });
            }
            catch (CustomException ex)
            {
                var innerEx = ex.InnerException == null ? "null" : ex.InnerException.Message;
                _logger.LogError(ex, innerEx);
                return BadRequest(new BaseResponse<string> { Code = ex.Code, Message = ex.Message, Data = innerEx });
            }
        }

        [HttpPut("Correction")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesErrorResponseType(typeof(BaseResponse<string>))]
        public async Task<IActionResult> UpdateTodoItem([FromBody] ToDoItemResponseModel request)
        {
            try
            {
                await _toDoItemService.UpdateToDoItemAsync(request.ToModel());

                return Ok(new BaseResponse<string> { Code = 0, Message = "Ok", Data = null });
            }
            catch (CustomException ex)
            {
                var innerEx = ex.InnerException == null ? "null" : ex.InnerException.Message;
                _logger.LogError(ex, innerEx);
                return BadRequest(new BaseResponse<string> { Code = ex.Code, Message = ex.Message, Data = innerEx });
            }
        }

        [HttpPost("Addition")]
        [ProducesResponseType(typeof(BaseResponse<ToDoItemResponseModel>), 200)]
        [ProducesErrorResponseType(typeof(BaseResponse<string>))]
        public async Task<ActionResult<ToDoItemResponseModel>> CreateTodoItem([FromBody] ToDoItemRequestModel request)
        {
            try
            {
                var res = await _toDoItemService.AddToDoItemAsync(request.ToModel());

                return Ok(new BaseResponse<ToDoItemResponseModel> { Code = 0, Message = "Ok", Data = new ToDoItemResponseModel(res) });
            }
            catch (CustomException ex)
            {
                var innerEx = ex.InnerException == null ? "null" : ex.InnerException.Message;
                _logger.LogError(ex, innerEx);
                return BadRequest(new BaseResponse<string> { Code = ex.Code, Message = ex.Message, Data = innerEx });
            }
        }

        [HttpDelete("Removing/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesErrorResponseType(typeof(BaseResponse<string>))]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await _toDoItemService.DeleteToDoItemAsync(id);

                return Ok(new BaseResponse<string> { Code = 0, Message = $"Задача с номером: '{id}' удалена.", Data = null });
            }
            catch (CustomException ex)
            {
                var innerEx = ex.InnerException == null ? "null" : ex.InnerException.Message;
                _logger.LogError(ex, innerEx);
                return BadRequest(new BaseResponse<string> { Code = ex.Code, Message = ex.Message, Data = innerEx });
            }
        }
    }
}
