namespace Todo.Infrastructure.Models.Entities;

public struct User
{
    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}