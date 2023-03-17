using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Interface;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItem>> GetAsync(CancellationToken cancellationToken = default);
    Task<int?> CreateAsync(TodoItem data);
    Task<bool> UpdateAsync(TodoItem id);
    Task<bool> DeleteAsync(int id);
}