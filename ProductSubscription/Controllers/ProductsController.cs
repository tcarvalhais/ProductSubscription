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
            var products = await productsRepository.GetAllProductsAsync();
            return products;
        }

        // Get product by id
        [HttpGet("getProductById/{productId}")]
        public async Task<ActionResult<ProductDTO>> GetProductAsync(Guid productId)
        {
            var product = await productsRepository.GetProductAsync(productId);
            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        // Get all products from a user
        [HttpGet("getProductsFromUser/{userId}")]
        public async Task<IEnumerable<ProductDTO>> GetAllProductsFromUserAsync(Guid userId)
        {
            IEnumerable<ProductDTO> products = new List<ProductDTO>();

            var user = await usersRepository.GetUserAsync(userId);
            if (user is not null)
            {
                products = await productsRepository.GetAllProductsFromUserAsync(userId);
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
                    products = products.Concat(listProducts);
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
                return NotFound();
            }

            var product = await productsRepository.CreateProductAsync(productDTO);
            return CreatedAtAction(nameof(GetProductAsync), new { productId = product.Id }, product.AsDTO());
        }

        // Delete a product
        [HttpDelete("deleteProduct/{productId}")]
        public async Task<ActionResult> DeleteProductAsync(Guid productId)
        {
            var existingProduct = await productsRepository.GetProductAsync(productId);
            if (existingProduct is null)
            {
                return NotFound();
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
                return NotFound();
            }

            await productsRepository.UpdateProductAsync(productId, productDTO);
            return NoContent();
        }
    }
}