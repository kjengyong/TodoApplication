using Microsoft.Extensions.Logging;
using System.Text.Json;
using Todo.Infrastructure.Interface;
using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Repositories;

/// <summary>
/// In real live application should using DB, to make it more easy to setup therefore store the data inside json file
/// </summary>
public class TodoItemRepository : ITodoItemRepository
{
    private readonly ILogger<TodoItemRepository> _logger;
    private readonly IFileRepository _fileRepository;
    private readonly string _filePath = @"TodoData.json";
    public TodoItemRepository(ILogger<TodoItemRepository> logger, IFileRepository fileRepository)
    {
        _logger = logger;
        _fileRepository = fileRepository;
    }

    public async Task<IEnumerable<TodoItem>> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<TodoItem>? data = await _fileRepository.Get<IEnumerable<TodoItem>>(_filePath, cancellationToken);
            return data ?? new List<TodoItem>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return new List<TodoItem>();
        }
    }

    public virtual async Task<int?> CreateAsync(TodoItem data, CancellationToken cancellationToken)
    {
        IEnumerable<TodoItem> todoEnumerable = await GetAsync();
        List<TodoItem> todoItems = todoEnumerable.ToList();
        int maxId = todoItems.Max(x => x.Id) ?? 0;
        int nextId = maxId + 1;
        try
        {
            data.Id = nextId;
            todoItems.Add(data);
            await _fileRepository.CreateOrUpdateFileAsync(_filePath, todoItems, cancellationToken);
            return nextId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return null;
        }
    }


    public async Task<bool> UpdateAsync(TodoItem updatedData, CancellationToken cancellationToken)
    {
        IEnumerable<TodoItem> todoEnumerable = await GetAsync();
        List<TodoItem> todoItems = todoEnumerable.ToList();
        if (!todoItems.Any())
            return false;
        TodoItem? dbRecord = todoItems.ToList().FirstOrDefault(data => data.Id == updatedData.Id);
        if (dbRecord == null)
            return false;
        dbRecord.SetData(updatedData);

        try
        {
            await _fileRepository.CreateOrUpdateFileAsync(_filePath, todoItems, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return false;
        }
    }
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        IEnumerable<TodoItem> todoEnumerable = await GetAsync();

        List<TodoItem> todoItems = todoEnumerable.ToList();
        if (!todoItems.Any())
            return false;
        todoItems = todoItems.Where(data => data.Id != id).ToList();
        try
        {
            await _fileRepository.CreateOrUpdateFileAsync(_filePath, todoItems, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return false;
        }
    }
}