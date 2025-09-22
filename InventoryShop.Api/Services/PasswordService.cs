

using Microsoft.AspNetCore.Identity;

public class PasswordService
{
    private readonly IPasswordHasher<string> _passwordHasher;


    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<string>();
    }



}