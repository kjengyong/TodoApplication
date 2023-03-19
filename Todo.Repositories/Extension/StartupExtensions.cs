using Microsoft.Extensions.DependencyInjection;
using Todo.Infrastructure.Interface;
using Todo.Infrastructure.Repositories;

namespace Todo.Infrastructure.Extension;

public static class StartupExtensions
{
    public static IServiceCollection AddTodoRepositoriesConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}