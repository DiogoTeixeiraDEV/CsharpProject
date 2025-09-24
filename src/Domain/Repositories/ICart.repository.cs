using Domain.Entities;


namespace Domain.Repositories;

public interface ICartRepository
{
    Task<Cart> CreateAsync(Cart cart);  // cria um cart e retorna o cart criado
    Task<Cart?> GetByIdAsync(Guid id); // busca pelo ID
    Task<Cart?> GetByUserIdAsync(Guid userId); // busca pelo UserID
    Task UpdateAsync(Cart cart); // atualiza um cart
    Task DeleteAsync(Guid id); // deleta um  cart pelo ID
}