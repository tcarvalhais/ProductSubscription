using Microsoft.AspNetCore.Mvc;
using ProductSubscription.DTOS;
using ProductSubscription.Models;
using ProductSubscription.Repositories;

namespace ProductSubscription.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository productsRepository;
        private readonly IUsersRepository usersRepository;

        public ProductsController(IProductsRepository productsRepository, IUsersRepository usersRepository)
        {
            this.productsRepository = productsRepository;
            this.usersRepository = usersRepository;
        }

        // Get all products
        [HttpGet("getAllProducts")]
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = (await productsRepository.GetAllProductsAsync()).Select(product => product.AsDTO());
            return products;
        }

        // Get product by id
        [HttpGet("getProductById/{productId}")]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(Guid id)
        {
            var product = await productsRepository.GetProductAsync(id);
            if (product is null)
            {
                return NotFound("Product not found");
            }

            return product.AsDTO();
        }

        // Get all products from a user
        [HttpGet("getProductsFromUser/{userId}")]
        public async Task<IEnumerable<ProductDTO>> GetAllProductsFromUserAsync(Guid userId)
        {
            IEnumerable<ProductDTO> products = new List<ProductDTO>();

            var user = await usersRepository.GetUserAsync(userId);
            if (user is not null)
            {
                var listProducts = await productsRepository.GetAllProductsFromUserAsync(userId);
                products = listProducts.Select(product => product.AsDTO());
            }

            return products;
        }

        // Get all products from subscribed users
        [HttpGet("getAllProductsFromSubscribedUsers/{userId}")]
        public async Task<IEnumerable<ProductDTO>> GetAllProductsFromSubscribedUsersAsync(Guid userId)
        {
            IEnumerable<ProductDTO> products = new List<ProductDTO>();

            var user = await usersRepository.GetUserAsync(userId);
            if (user is not null)
            {
                List<User> subscribedUsers = (await usersRepository.GetAllSubscribedUsersAsync(userId)).ToList();
                foreach (var subscribedUser in subscribedUsers)
                {
                    var listProducts = await productsRepository.GetAllProductsFromUserAsync(subscribedUser.Id);
                    var productsFromUser = listProducts.Select(product => product.AsDTO());
                    products = products.Concat(productsFromUser);
                }
            }

            return products;
        }

        // Create a new product
        [HttpPost("createProduct")]
        public async Task<ActionResult<ProductDTO>> CreateProductAsync(CreateProductDTO productDTO)
        {
            var user = await usersRepository.GetUserAsync(productDTO.CreatorUserId);
            if (user is null)
            {
                return NotFound("User not found");
            }

            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = productDTO.Name,
                CreatorUserId = productDTO.CreatorUserId,
                Price = productDTO.Price
            };

            await productsRepository.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductAsync), new { id = product.Id }, product.AsDTO());
        }

        // Delete a product
        [HttpDelete("deleteProduct/{productId}")]
        public async Task<ActionResult> DeleteProductAsync(Guid productId)
        {
            var existingProduct = await productsRepository.GetProductAsync(productId);
            if (existingProduct is null)
            {
                return NotFound("Product not found");
            }

            await productsRepository.DeleteProductAsync(productId);
            return NoContent();
        }

        // Update a price of a product
        [HttpPut("updatePrice/{productId}")]
        public async Task<ActionResult> UpdateProductAsync(Guid productId, UpdateProductDTO productDTO)
        {
            var existingProduct = await productsRepository.GetProductAsync(productId);
            if (existingProduct is null)
            {
                return NotFound("Product not found");
            }

            Product updatedProduct = existingProduct with
            {
                Price = productDTO.Price
            };

            await productsRepository.UpdateProductAsync(updatedProduct);
            return NoContent();
        }
    }
}