using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.dtos;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("auth")]
[SwaggerTag("Endpoints para autenticação e registro de usuários")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Registra um novo usuário", 
        Description = "Registra um novo usuário com os detalhes fornecidos."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(void))]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        try
        {
            var user = await _authService.RegisterAsync(dto);
            return Ok(new { user.Id, user.Name, user.Email, user.Role });
        }
        catch (Exception)
        {
            // Retorna status sem corpo
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Autentica um usuário", 
        Description = "Autentica um usuário e retorna um token JWT."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch (Exception)
        {
            // Retorna status sem corpo
            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
