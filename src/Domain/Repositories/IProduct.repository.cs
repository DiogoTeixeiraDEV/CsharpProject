using Domain.Entities;


public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);  // cria um produto e retorna o produto criado
    Task<Product?> GetByIdAsync(Guid id); // busca pelo ID
    Task<IEnumerable<Product>> GetAllAsync(); // busca todos os produtos
    Task UpdateAsync(Product product); // atualiza um produto
    Task DeleteAsync(Guid id); // deleta um  produto pelo ID
}