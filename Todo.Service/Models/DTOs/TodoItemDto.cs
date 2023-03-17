using System.ComponentModel.DataAnnotations;
using Todo.Core.Enums;

namespace Todo.Service.Models.DTOs;
/// <summary>
/// Todo item model
/// </summary>
public class TodoItemDto
{
    /// <summary>
    /// Todo item id
    /// </summary>
    public int? Id { get; set; }
    /// <summary>
    /// Title of the todo item
    /// </summary>
    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(100)]
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
    [Required(ErrorMessage = "Status cannot be empty")]
    public TodoItemStatus Status { get; set; }

    /// <summary>
    /// dynamic object
    /// </summary>
    public dynamic? Data { get; set; }
}