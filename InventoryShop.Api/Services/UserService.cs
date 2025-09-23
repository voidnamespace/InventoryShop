namespace IS.UserService;
using IS.PasswordService;
using IS.DTOs;
using Microsoft.AspNetCore.Mvc;
using IS.DbContext;
using IS.Entities;
using Microsoft.EntityFrameworkCore;


public class UserService 
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly PasswordService _passwordService;

    public UserService (AppDbContext context, ILogger<UserService> logger, PasswordService passwordService)
    {
        _context = context;
        _logger = logger;
        _passwordService = passwordService;
    }

    
    public async Task<List<ReadUserDTO>> GetAllAsync()
    {
        _logger.LogInformation("Request to get all users");

        var users = await _context.Users.ToListAsync();

        if (!users.Any())
            _logger.LogInformation("Users not found");

        var usersToDTO = users.Select(x => new ReadUserDTO
        {
            UserName = x.UserName,
            Email = x.Email,
        }).ToList();

        return usersToDTO;

    }

    public async Task <ReadUserDTO> GetById (Guid id)
    {
        _logger.LogInformation("Request to get user with {id}", id);

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            _logger.LogWarning("User with id {id} not found ", id);
            throw new KeyNotFoundException("User not found");
        }
          

        var readUserDTO = new ReadUserDTO
        {
            UserName = user.UserName,
            Email = user.Email
        };

        return readUserDTO;

    }

    public async Task <ReadUserDTO> PostAsync (PostUserDTO postUserDTO)
    {
        if (postUserDTO == null)
            throw new ArgumentNullException("User can not be null");

        _logger.LogInformation("Request to post new user");
        

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = postUserDTO.UserName,
            Email = postUserDTO.Email
            
        };
        var password = postUserDTO.Password;
        user.PasswordHash = _passwordService.HashPassword(user, password);
        await _context.AddAsync(user);

        await _context.SaveChangesAsync();

        _logger.LogInformation("User created with id {userID}", user.Id);

        var readUserDTO = new ReadUserDTO
        {
           Id = user.Id, 
           UserName = user.UserName,
           Email = user.Email,
        };
        return readUserDTO;
    }

    public async Task<bool> DeleteAsync (Guid id)
    {
        var delUser = await _context.Users.FindAsync(id);

        if (delUser == null)
        {
            _logger.LogWarning("Request to delete non-existing user with id {userId}", id);
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        _logger.LogInformation("Request to delete user with id {userId}", id);

        _context.Users.Remove(delUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User with id {userId} has been deleted", id);
        return true;
    }







}