using Todo.Core.Enums;
using Todo.Service.Models.DTOs;

namespace Todo.Service.Interface;

public interface ITodoItemService
{
    Task<IEnumerable<TodoItemDto>> GetAsync(SortType? sortType = null, string? searchKeyword = null, CancellationToken cancellationToken = default);
    Task<int?> CreateAsync(TodoItemDto data, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(TodoItemDto data, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}