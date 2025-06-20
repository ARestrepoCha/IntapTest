using FluentResults;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;

namespace IntapTest.Domain.Services.Users
{
    public interface IUserService
    {
        Task<Result<bool>> CreateUser(CreateUserRequestDto createUserRequest);
        Task<Result<LoginResponseDto>> Login(LoginUserRequestDto loginUser);
    }
}
