using System;

namespace Domain.Entities;

public enum UserRole
{
    Admin,
    User
}

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;

    public UserRole Role { get; private set; } = UserRole.User;
    public Cart Cart { get; private set; } = null!;

    private User() { }


    public User(string name, string email, string password, UserRole role = UserRole.User)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }


    public void Update(string name, string email, string password, UserRole? role = null)
    {
        Name = name;
        Email = email;
        Password = password;
        if(role.HasValue)
        {
            Role = role.Value;
        }
    }
}

