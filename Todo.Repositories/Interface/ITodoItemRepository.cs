using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Interface;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItem>> GetAsync(CancellationToken cancellationToken = default);
    Task<int?> CreateAsync(TodoItem data, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(TodoItem id, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}