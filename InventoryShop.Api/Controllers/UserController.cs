namespace IS.Controller;
using IS.DTOs;
using IS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController (UserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("users")]
    public async Task<ActionResult> GetAll()
    {
        var allReadUserDTO = await _userService.GetAllAsync();
        if (allReadUserDTO == null) 
            return NotFound("Users not found");
        return Ok(allReadUserDTO);  
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task <ActionResult> GetById(Guid id)
    {
        var readUserDTO = await _userService.GetById(id);
        return Ok(readUserDTO);
    }
    [Authorize]
    [HttpPost]
    [Route("post")]
    public async Task<ActionResult<ReadUserDTO>> Post (PostUserDTO postUserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var post = await _userService.PostAsync(postUserDTO);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    public async Task<ActionResult> Put (Guid id, PutUserDTO putUserDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var put = await _userService.PutAsync(id, putUserDTO);
        return Ok(put);
    }





    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (Guid id)
    {
        var delete = await _userService.DeleteAsync(id);
        return NoContent();
    }

}
