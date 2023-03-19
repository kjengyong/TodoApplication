using Todo.Infrastructure.Models.Entities;

namespace Todo.Infrastructure.Interface;

public interface IFileRepository
{
    Task CreateOrUpdateFileAsync<T>(string filePath, T data, CancellationToken cancellationToken);
    Task<T?> Get<T>(string filePath, CancellationToken cancellationToken);
}