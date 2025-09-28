namespace IS.DTOs;
using System.ComponentModel.DataAnnotations;

public class PutUserDTO
{
    [MaxLength(25)]
    public string? UserName { get; set; } 

    [EmailAddress]
    [MaxLength(50)]
    public string? Email { get; set; } 

    public string? Password { get; set; }
}