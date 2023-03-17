using MapsterMapper;
using Todo.Core.Enums;
using Todo.Infrastructure.Interface;
using Todo.Service.Interface;
using Todo.Service.Models.DTOs;
using RepositoriesModels = Todo.Infrastructure.Models.Entities;

namespace Todo.Service.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IMapper _iMapper;
    public TodoItemService(ITodoItemRepository todoItemRepository, IMapper iMapper)
    {
        _todoItemRepository = todoItemRepository;
        _iMapper = iMapper;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAsync(SortType? sortType = null, string? searchKeyword = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<TodoItemDto> data = _iMapper.Map<IEnumerable<TodoItemDto>>(await _todoItemRepository.GetAsync(cancellationToken));
        data = FilterTodo(searchKeyword, data);
        return SortTodo(sortType, data);
    }

    public async Task<int?> CreateAsync(TodoItemDto data)
    {
        return await _todoItemRepository.CreateAsync(_iMapper.Map<RepositoriesModels.TodoItem>(data));
    }

    public async Task<bool> UpdateAsync(TodoItemDto data)
    {
        return await _todoItemRepository.UpdateAsync(_iMapper.Map<RepositoriesModels.TodoItem>(data));
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _todoItemRepository.DeleteAsync(id);
    }

    #region Private method
    private static IEnumerable<TodoItemDto> FilterTodo(string? searchKeyword, IEnumerable<TodoItemDto> data)
    {
        if (!string.IsNullOrWhiteSpace(searchKeyword))
            data = data.Where(x => x.Name.Contains(searchKeyword, StringComparison.CurrentCulture)
                                   || x.Description?.Contains(searchKeyword, StringComparison.CurrentCulture) == true);
        return data;
    }

    private static IEnumerable<TodoItemDto> SortTodo(SortType? sortType, IEnumerable<TodoItemDto> data)
    {
        switch (sortType)
        {
            case SortType.DueDate:
                return data.OrderBy(x => x.DueDate);
            case SortType.DueDateDesc:
                return data.OrderByDescending(x => x.DueDate);
            case SortType.Name:
                return data.OrderBy(x => x.Name);
            case SortType.NameDesc:
                return data.OrderByDescending(x => x.Name);
            case SortType.Status:
                return data.OrderBy(x => x.Status);
            case SortType.StatusDesc:
                return data.OrderByDescending(x => x.Status);
            default:
                return data;
        }
    }
    #endregion
}