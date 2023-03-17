using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Interface;

public interface IUserRepository
{
    Task<bool> Create(User data);
    Task<bool> Login(User data);
}