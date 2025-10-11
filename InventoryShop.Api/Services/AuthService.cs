namespace IS.Services;
using IS.DTOs;
using IS.DbContext;
using IS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordService _hasher;
    private readonly ILogger<AuthService> _logger;
    private readonly TokenService _token;

    public AuthService (AppDbContext context, PasswordService hasher, ILogger<AuthService> logger, TokenService token)
    {
        _context = context;
        _hasher = hasher;
        _logger = logger;
        _token = token;
    }
    public async Task<LoginResponseDTO?> LoginAsync (string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            _logger.LogWarning("Login failed: user with email {Email} not found", email);
            return null; 
        }
        var isValid = await _hasher.VerifyPassword(user, user.PasswordHash, password);
        if (!isValid)
        {
            _logger.LogWarning("Login failed: incorrect password for user {Email}", email);
            return null;
        }

        _logger.LogInformation("User {UserId} logged in successfully", user.Id);

        var token = _token.GenerateToken(user);

        return new LoginResponseDTO
        {
            Token = token,
            UserId = user.Id,
            Username = user.UserName,
            Role = user.Role.ToString()
        };

    }
    public async Task<ReadUserDTO> RegisterAsync (string username, string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email))
            throw new ArgumentException("Email already exists");

        if (await _context.Users.AnyAsync(u => u.UserName == username))
            throw new ArgumentException("Username already exists");

        _logger.LogInformation("Request to create new user");

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = email,
            Role = Enums.Roles.Customer
        };
        var hashPas = _hasher.HashPassword(newUser, password);
        newUser.PasswordHash = hashPas;

        await _context.Users.AddAsync(newUser);

        await _context.SaveChangesAsync();

        _logger.LogInformation("User with {UserId} successfully created", newUser.Id);

        return new ReadUserDTO
        {
            Id = newUser.Id,
            UserName = newUser.UserName,
            Email = newUser.Email,
        };
    }
}
