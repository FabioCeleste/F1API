using ErrorOr;
using F1RestAPI.Contracts.Users;

namespace F1RestAPI.Services.Users
{
    public interface IUserService
    {
        Task<bool> CreateUser(CreateUserRequest createUserRequest);
        Task<ErrorOr<string>> Login(CreateUserRequest loginRequest);
    }
}