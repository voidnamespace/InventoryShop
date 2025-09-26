namespace IS.DTOs;
using Microsoft.AspNetCore.Identity;
using IS.Enums;


public class ReadUserDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;

}