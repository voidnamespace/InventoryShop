namespace IS.Controller;

using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController (AuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ReadUserDTO>> Register([FromBody] PostUserDTO dto)
    {
        var user = await _authService.RegisterAsync(dto.UserName, dto.Email, dto.Password);
        return CreatedAtAction(null, new { id = user.Id }, user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Password);

        if (result == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(result);
    }


}
