using Domain.Entities;
using Domain.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.dtos;
using System.Security.Claims;

namespace Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly string _jwtSecret;

    public AuthService(IUserRepository userRepository, string jwtSecret)
    {
        _userRepository = userRepository;
        _jwtSecret = jwtSecret;
    }

    public async Task<User> RegisterAsync(UserRegisterDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null) throw new Exception("User already exists");

        var hashedPassword = HashPassword(dto.Password);
        
        var user = new User(dto.Name, dto.Email, hashedPassword, dto.Role ?? UserRole.User);

        return await _userRepository.CreateAsync(user);
    }

    public async Task<string?> LoginAsync(UserLoginDto dto)
    {
       
        
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !VerifyPassword(dto.Password, user.Password))
            throw new Exception("Invalid email or password");

        return GenerateJwtToken(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hashedPassword;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim("email", user.Email),
                new System.Security.Claims.Claim("role", user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}