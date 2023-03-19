using Todo.Service.Models.DTOs;

namespace Todo.Service.Interface;

public interface IUserService
{
    Task<bool> Create(UserDto data, CancellationToken cancellationToken);
    Task<string?> Login(UserDto data, CancellationToken cancellationToken);
}