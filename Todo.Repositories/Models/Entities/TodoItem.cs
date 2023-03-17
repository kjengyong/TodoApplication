using Todo.Core.Enums;

namespace Todo.Infrastructure.Models.Entities;
/// <summary>
/// Todo item model
/// </summary>
public class TodoItem
{
    /// <summary>
    /// Todo item id
    /// </summary>
    public int? Id { get; set; }
    /// <summary>
    /// Title of the todo item
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public TodoItemStatus Status { get; set; }


    /// <summary>
    /// dynamic object
    /// </summary>
    public dynamic? Data { get; set; }

    public void SetData(TodoItem data)
    {
        Name = data.Name;
        Description = data.Description;
        DueDate = data.DueDate;
        Status = data.Status;
        Data = data.Data;
    }
}