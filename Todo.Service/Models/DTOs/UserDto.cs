using System.ComponentModel.DataAnnotations;

namespace Todo.Service.Models.DTOs;

public struct UserDto
{
    [Required(ErrorMessage = "User name cannot be empty")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password cannot be empty")]
    public string Password { get; set; }
}