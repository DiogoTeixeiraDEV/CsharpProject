using System;


namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;

    private User() { }


    public User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
    }
    

    public void Update(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

}

