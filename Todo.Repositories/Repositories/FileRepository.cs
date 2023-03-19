using Microsoft.Extensions.Logging;
using System.Text.Json;
using Todo.Infrastructure.Interface;
using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Repositories;

/// <summary>
/// In real live application should using DB, to make it more easy to setup therefore store the data inside json file
/// </summary>
public class FileRepository : IFileRepository
{
    private readonly ILogger<TodoItemRepository> _logger;
    public FileRepository(ILogger<TodoItemRepository> logger)
    {
        _logger = logger;
    }

    public async Task<T?> Get<T>(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(filePath, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateOrUpdateFileAsync));
            throw;
        }
    }

    public async Task CreateOrUpdateFileAsync<T>(string filePath, T data, CancellationToken cancellationToken)
    {
        try
        {
            if (File.Exists(filePath))
            {
                using FileStream updateStream = File.Open(filePath, FileMode.Truncate);
                await JsonSerializer.SerializeAsync(updateStream, data, typeof(T), cancellationToken: cancellationToken);
            }
            else
            {
                using FileStream createStream = File.Create(filePath);
                await JsonSerializer.SerializeAsync(createStream, data, typeof(T), cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(CreateOrUpdateFileAsync));
            throw;
        }
    }
}