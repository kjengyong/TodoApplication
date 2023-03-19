using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Todo.API.Models.Response;
using Todo.Core.Enums;
using Todo.Service.Interface;
using Todo.Service.Models.DTOs;

namespace Todo.API.Controllers
{
    /// <summary>
    /// Todo controller
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly ILogger<TodoController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="todoItemService"></param>
        /// <param name="logger"></param>
        public TodoController(ITodoItemService todoItemService, ILogger<TodoController> logger)
        {
            _todoItemService = todoItemService;
            _logger = logger;
        }

        /// <summary>
        /// GetAsync list of todo item
        /// </summary>
        /// <param name="sortType"></param>
        /// <param name="searchKeyword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TodoItemDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync(SortType? sortType = null, string? searchKeyword = null, CancellationToken cancellationToken = default)
        {
            try
            {
                List<TodoItemDto> response = (await _todoItemService.GetAsync(sortType, searchKeyword, cancellationToken)).ToList();
                return response.Any()
                    ? Ok(new BaseResponse<IEnumerable<TodoItemDto>>(response))
                    : (IActionResult)StatusCode(StatusCodes.Status204NoContent, new BaseResponse(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAsync));
                return BadRequest(new BaseResponse());
            }
        }

        /// <summary>
        /// CreateAsync todo item
        /// data is an dynamic object
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///      "id": 0,
        ///      "name": "shopping",
        ///      "description": "buy milk",
        ///      "dueDate": "2023-03-18T01:50:34.858Z",
        ///      "status": "Backlog",
        ///      "data": {
        ///       "test":"test"
        ///      }
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<int?>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] TodoItemDto request, CancellationToken cancellationToken)
        {
            try
            {
                int? id = await _todoItemService.CreateAsync(request, cancellationToken);
                if (id != null)
                    return Ok(new BaseResponse<int?>(id));
                return BadRequest(new BaseResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateAsync));
                return BadRequest(new BaseResponse());
            }
        }

        /// <summary>
        /// UpdateAsync todo item
        /// data is an dynamic object
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///      "id": 1,
        ///      "name": "shopping",
        ///      "description": "buy milk",
        ///      "dueDate": "2023-03-18T01:50:34.858Z",
        ///      "status": "Backlog",
        ///      "data": {
        ///       "test":"test"
        ///      }
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] TodoItemDto request, CancellationToken cancellationToken)
        {
            try
            {
                bool isSuccess = await _todoItemService.UpdateAsync(request, cancellationToken);
                return isSuccess ? Ok(new BaseResponse(true)) : BadRequest(new BaseResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(UpdateAsync));
                return BadRequest(new BaseResponse());
            }
        }

        /// <summary>
        /// DeleteAsync todo item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromBody] int id, CancellationToken cancellationToken)
        {
            try
            {
                bool isSuccess = await _todoItemService.DeleteAsync(id, cancellationToken);
                return isSuccess ? Ok(new BaseResponse(true)) : BadRequest(new BaseResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
                return BadRequest(new BaseResponse());
            }
        }
    }
}
