using Domain.Entities;

namespace Application.dtos;


public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }

}
