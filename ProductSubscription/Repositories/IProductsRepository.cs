using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> GetProductAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetAllProductsFromUserAsync(Guid userId);
        Task CreateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task DeleteAllProductsFromUserAsync(Guid userId);
        Task UpdateProductAsync(Product product);
    }
}