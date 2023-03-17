using Microsoft.Extensions.DependencyInjection;
using Todo.Service.Interface;
using Todo.Service.Services;
using Todo.Infrastructure.Extension;
using Mapster;
using MapsterMapper;

namespace Todo.Service.Extension;

public static class StartupExtensions
{
    public static IServiceCollection AddTodoServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IMapper, Mapper>();
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
        TypeAdapterConfig.GlobalSettings.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
        TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        services.AddTodoRepositoriesConfiguration();

        services.AddScoped<ITodoItemService, TodoItemService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}