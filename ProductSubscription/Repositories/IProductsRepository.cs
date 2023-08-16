using ProductSubscription.DTOS;
using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public interface IProductsRepository
    {
        Task<ProductDTO> GetProductAsync(Guid id);
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetAllProductsFromUserAsync(Guid userId);
        Task<Product> CreateProductAsync(CreateProductDTO productDTO);
        Task DeleteProductAsync(Guid id);
        Task DeleteAllProductsFromUserAsync(Guid userId);
        Task UpdateProductAsync(Guid productId, UpdateProductDTO productDTO);
    }
}