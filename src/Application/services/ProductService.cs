using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;


public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> CreateProductAsync(string name, string description, decimal price, string category)
    {
        var product = new Product(name, description, price, category);
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task UpdateProductAsync(Guid id, string name, string description, decimal price, string category)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            product.Update(name, description, price, category);
            await _productRepository.UpdateAsync(product);
        }
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;
        await _productRepository.DeleteAsync(product.Id);
        return true;
    }
}