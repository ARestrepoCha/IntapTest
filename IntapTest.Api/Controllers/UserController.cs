using FluentResults;
using IntapTest.Domain.Services.Users;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using IntapTest.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace IntapTest.Api.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]

        public async Task<ActionResult<Result<bool>>> CreateUser([FromBody] CreateUserRequestDto createUserRequest)
        {
            var result = await _userService.CreateUser(createUserRequest);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<Result<LoginResponseDto>>> Login(LoginUserRequestDto loginDto)
        {
            var result = await _userService.Login(loginDto);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }
    }
}
