using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Services;
using Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        var usersDto = users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        });
        return Ok(usersDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        var userDto = new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
    {
        var user = await _userService.CreateUserAsync(dto.Name, dto.Email, dto.Password);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto dto)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null) return NotFound();

        await _userService.UpdateUserAsync(id, dto.Name, dto.Email, dto.Password);
        return NoContent();
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
    
}
    
    