using System;
using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);  // cria um user e retorna o user criado
    Task<User?> GetByIdAsync(Guid id); // busca pelo ID
    Task<User?> GetByEmailAsync(string email);// busca pelo Email
    Task<IEnumerable<User>> GetAllAsync(); // busca todos os users
    Task UpdateAsync(User user); // atualiza um user
    Task DeleteAsync(Guid id); // deleta um  user pelo ID
}