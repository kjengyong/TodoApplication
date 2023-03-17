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
    private readonly string _filePath = @"TodoData.json";
    public TodoItemRepository(ILogger<TodoItemRepository> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TodoItem>> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            string json = await File.ReadAllTextAsync(_filePath, cancellationToken);
            IEnumerable<TodoItem>? data = JsonSerializer.Deserialize<IEnumerable<TodoItem>>(json);
            return data ?? new List<TodoItem>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return new List<TodoItem>();
        }
    }

    public virtual async Task<int?> CreateAsync(TodoItem data)
    {
        IEnumerable<TodoItem> todoEnumerable = await GetAsync();
        List<TodoItem> todoItems = todoEnumerable.ToList();
        int maxId = todoItems.Max(x => x.Id) ?? 0;
        int nextId = maxId + 1;
        try
        {
            data.Id = nextId;
            todoItems.Add(data);

            await using FileStream createStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(createStream, todoItems);
            return nextId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return null;
        }
    }


    public async Task<bool> UpdateAsync(TodoItem updatedData)
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
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(todoItems.ToList()));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return false;
        }
    }
    public async Task<bool> DeleteAsync(int id)
    {
        IEnumerable<TodoItem> todoEnumerable = await GetAsync();

        List<TodoItem> todoItems = todoEnumerable.ToList();
        if (!todoItems.Any())
            return false;
        todoItems = todoItems.Where(data => data.Id != id).ToList();
        try
        {
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(todoItems));
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateAsync));
            return false;
        }
    }

}