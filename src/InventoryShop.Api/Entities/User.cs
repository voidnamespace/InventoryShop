using Microsoft.AspNetCore.Identity;
using IS.Enums;

namespace IS.Entities;


public class Order
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; }

}