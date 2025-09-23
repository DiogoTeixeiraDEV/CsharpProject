using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using Application.dtos;

namespace Api.Controllers;

[ApiController]
[Route("users")]
[SwaggerTag("Endpoints para gerenciamento de usuários")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Obtém uma lista de todos os usuários registrados no sistema.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Busca um usuário pelo ID"), Description("Obtém os detalhes de um usuário específico usando seu ID. **Apenas ADMIN**")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Cria um novo usuário", Description = "Cria um novo usuário com os detalhes fornecidos. **Apenas ADMIN**")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
    {
        var user = await _userService.CreateUserAsync(dto.Name, dto.Email, dto.Password);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Atualiza um usuário existente", Description = "Atualiza os detalhes de um usuário existente usando seu ID. **Apenas ADMIN**")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto dto)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null) return NotFound();

        await _userService.UpdateUserAsync(id, dto.Name, dto.Email, dto.Password);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Deleta um usuário", Description = "Deleta um usuário específico usando seu ID. **Apenas ADMIN**")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
    
}
    
    