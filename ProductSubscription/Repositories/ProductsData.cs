using ProductSubscription.DTOS;
using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public class ProductsData : IProductsRepository
    {
        private List<Product> products = new()
        {
            new Product { Id = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204"), Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 },
            new Product { Id = new Guid("a7f41a2f-9738-49a2-92cf-2de10f58f286"), Name = "Sømods Bolcher Bonbons", CreatorUserId = new Guid("022cd49d-6e41-49f3-9644-dbbfadd2abe8"), Price = 5.99 },
            new Product { Id = new Guid("f87ddbce-4fb5-44c5-950c-abeb35bf02ec"), Name = "Georg Jensen Jewelry", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 1620.00 },
            new Product { Id = new Guid("9e8d187f-72cd-4347-b2ba-65dad3daf224"), Name = "PH Lamps", CreatorUserId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Price = 525.55 }
        };

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return await Task.FromResult(products.Select(product => product.AsDTO()));
        }

        public async Task<ProductDTO> GetProductAsync(Guid id)
        {
            var product = products.Where(product => product.Id == id).SingleOrDefault();
            return await Task.FromResult(product?.AsDTO());
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsFromUserAsync(Guid userId)
        {
            var listProducts = products.Where(product => product.CreatorUserId == userId).Select(product => product.AsDTO());
            return await Task.FromResult(listProducts);
        }

        public async Task<Product> CreateProductAsync(CreateProductDTO productDTO)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = productDTO.Name,
                CreatorUserId = productDTO.CreatorUserId,
                Price = productDTO.Price
            };

            products.Add(product);
            return await Task.FromResult(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var index = products.FindIndex(existingProduct => existingProduct.Id == id);
            products.RemoveAt(index);

            await Task.CompletedTask;
        }

        public async Task DeleteAllProductsFromUserAsync(Guid userId)
        {
            products.RemoveAll(product => product.CreatorUserId == userId);
            await Task.CompletedTask;
        }

        public async Task UpdateProductAsync(Guid productId, UpdateProductDTO productDTO)
        {
            var index = products.FindIndex(existingProduct => existingProduct.Id == productId);
            Product updatedProduct = products[index] with
            {
                Price = productDTO.Price
            };

            products[index] = updatedProduct;
            await Task.CompletedTask;
        }
    }
}