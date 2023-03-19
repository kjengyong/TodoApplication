using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Models.Response;
using Todo.Core.Exception;
using Todo.Service.Interface;
using Todo.Service.Models.DTOs;

namespace Todo.API.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /LoginAsync
        ///     {
        ///        "Username": "Tester",
        ///        "Password": "Testing123"
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("LoginAsync")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync(UserDto user, CancellationToken cancellationToken)
        {
            try
            {
                string? token = await _userService.Login(user, cancellationToken);
                if (!string.IsNullOrWhiteSpace(token))
                    return Ok(new BaseResponse<string>(token));
                return Unauthorized();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, nameof(LoginAsync));
                return BadRequest(new BaseResponse());
            }
        }

        /// <summary>
        /// AddAsync member
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync(UserDto user, CancellationToken cancellationToken)
        {
            try
            {
                if (await _userService.Create(user, cancellationToken))
                    return Ok(new BaseResponse() { IsSuccess = true });
                return BadRequest(new BaseResponse() { Message = "Register Failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(AddAsync));
                if (ex is UserExistException)
                    return BadRequest(new BaseResponse() { Message = "User existed" });
                return BadRequest(new BaseResponse());
            }
        }
    }
}
