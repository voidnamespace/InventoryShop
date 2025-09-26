namespace IS.DTOs;
using Microsoft.AspNetCore.Identity;
using IS.Enums;


public class PostUserDTO
{
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;

    public string Password {  get; set; } = String.Empty;
}