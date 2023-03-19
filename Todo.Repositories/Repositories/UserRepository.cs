using Microsoft.Extensions.Logging;
using System.Text.Json;
using Todo.Infrastructure.Interface;
using Todo.Infrastructure.Models.Entities;
using System.Linq;
using Todo.Core.Exception;
using System.Security.Cryptography;
using System.Text;

namespace Todo.Infrastructure.Repositories;

/// <summary>
/// In real live application should using DB, to make it more easy to setup therefore store the data inside json file
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IFileRepository _fileRepository;
    private readonly string _filePath = @"UserData.json";
    public UserRepository(ILogger<UserRepository> logger, IFileRepository fileRepository)
    {
        _logger = logger;
        _fileRepository = fileRepository;
    }

    public async Task<bool> Create(User data, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await Get(cancellationToken);
        List<User> usersList = users.ToList();
        IsUserExist(data, usersList);

        try
        {
            data.Id = Guid.NewGuid().ToString();
            usersList.Add(data);
            await _fileRepository.CreateOrUpdateFileAsync(_filePath, usersList, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            if (ex is UserExistException)
            {
                throw;
            }
            _logger.LogError(ex, nameof(Create));
            return false;
        }
    }

    public async Task<bool> Login(User data, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<User> users = await Get(cancellationToken);
            return users.Any(x => (x.UserName?.Equals(data.UserName, StringComparison.OrdinalIgnoreCase)).GetValueOrDefault()
                             && x.Password!.Equals(data.Password));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Create));
            return false;
        }
    }

    private async Task<IEnumerable<User>> Get(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<User>? data = await _fileRepository.Get<IEnumerable<User>>(_filePath, cancellationToken);
            return data ?? new List<User>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Create));
            return new List<User>();
        }
    }
    private static void IsUserExist(User data, List<User> usersList)
    {
        if (usersList.Any(x => x.UserName?.Equals(data.UserName, StringComparison.OrdinalIgnoreCase) == true))
        {
            throw new UserExistException();
        }
    }
}