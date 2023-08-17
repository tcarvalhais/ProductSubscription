using FakeItEasy;
using ProductSubscription.Repositories;
using ProductSubscription.DTOS;
using ProductSubscription.Controllers;
using ProductSubscription.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductSubscription.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly IProductsRepository productsRepository;
    private readonly IUsersRepository usersRepository;

    public ProductsControllerTests()
    {
        productsRepository = A.Fake<IProductsRepository>();
        usersRepository = A.Fake<IUsersRepository>();
    }

    [Fact]
    public async void ProductsController_GetAllProductsAsync_ReturnSuccess()
    {
        // Arrange
        var testProduct = new Product { Id = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204"), Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetAllProductsAsync()).Returns(new List<Product> { testProduct });

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetAllProductsAsync();

        // Assert
        var resultProducts = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(result);
        Assert.Single(resultProducts);
    }

    [Fact]
    public async Task ProductsController_GetProductAsync_ProductExists()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetProductAsync(productId);

        // Assert
        var resultProduct = Assert.IsAssignableFrom<ActionResult<ProductDTO>>(result);
        Assert.Equal(productId, resultProduct.Value.Id);
    }

    [Fact]
    public async Task ProductsController_GetProductAsync_ProductNotFound()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((Product)null);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetProductAsync(productId);

        // Assert
        var resultProduct = Assert.IsAssignableFrom<ActionResult<ProductDTO>>(result);
        Assert.IsType<NotFoundObjectResult>(resultProduct.Result);
    }

    [Fact]
    public async Task ProductsController_GetAllProductsFromUserAsync_ReturnSuccess()
    {
        // Arrange
        var userId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var testUser = new User { Id = userId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        var listProducts = new List<Product>() { testProduct };

        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);
        A.CallTo(() => productsRepository.GetAllProductsFromUserAsync(userId)).Returns(listProducts);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetAllProductsFromUserAsync(userId);

        // Assert
        var productDTOs = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(result);
        Assert.Equal(listProducts.Count, productDTOs.Count());
    }

    [Fact]
    public async Task ProductsController_GetAllProductsFromSubscribedUsersAsync_ReturnSuccess()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var subscribedUser = new User { Id = subscribedUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        var listSubscribedUsers = new List<User>() { subscribedUser };

        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        var listProducts = new List<Product>() { testProduct };

        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);
        A.CallTo(() => usersRepository.GetAllSubscribedUsersAsync(userId)).Returns(listSubscribedUsers);
        A.CallTo(() => productsRepository.GetAllProductsFromUserAsync(subscribedUserId)).Returns(listProducts);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetAllProductsFromSubscribedUsersAsync(userId);

        // Assert
        var productDTOs = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(result);
        Assert.Equal(listSubscribedUsers.Count * listProducts.Count, productDTOs.Count());
    }

    [Fact]
    public async Task ProductsController_CreateProductAsync_UserExists()
    {
        // Arrange
        var testProductDTO = new CreateProductDTO { Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        var testUser = new User { Id = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(testProductDTO.CreatorUserId)).Returns(testUser);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.CreateProductAsync(testProductDTO);

        // Assert
        Assert.IsType<ActionResult<ProductDTO>>(result);
        if (result.Result is CreatedAtActionResult createdAtAction)
        {
            Assert.IsType<ProductDTO>(createdAtAction.Value);
        }
        else
        {
            Assert.True(false, "Expected a CreatedAtActionResult.");
        }

        A.CallTo(() => productsRepository.CreateProductAsync(A<Product>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ProductsController_CreateProductAsync_UserNotExist()
    {
        // Arrange
        var testProductDTO = new CreateProductDTO { Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        var testUser = new User { Id = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(testProductDTO.CreatorUserId)).Returns((User)null);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.CreateProductAsync(testProductDTO);

        // Assert
        Assert.IsType<ActionResult<ProductDTO>>(result);
        Assert.IsType<NotFoundObjectResult>(result.Result);
        A.CallTo(() => productsRepository.CreateProductAsync(A<Product>.Ignored)).MustNotHaveHappened();
    }

    [Fact]
    public async Task ProductsController_DeleteProductAsync_ExistingProduct()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.DeleteProductAsync(productId);

        // Assert
        A.CallTo(() => productsRepository.DeleteProductAsync(productId)).MustHaveHappenedOnceExactly();
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ProductsController_DeleteProductAsync_ProductNotFound()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((Product)null);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.DeleteProductAsync(productId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ProductsController_UpdateProductAsync_ExistingProduct()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct);

        // Act
        var productDTO = new UpdateProductDTO { Price = 10 };
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.UpdateProductAsync(productId, productDTO);

        // Assert
        Product updatedProduct = testProduct with
        {
            Price = productDTO.Price
        };

        A.CallTo(() => productsRepository.UpdateProductAsync(updatedProduct)).MustHaveHappenedOnceExactly();
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ProductsController_UpdateProductAsync_ProductNotFound()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((Product)null);

        // Act
        var productDTO = new UpdateProductDTO { Price = 10 };
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.UpdateProductAsync(productId, productDTO);

        // Assert
        Product updatedProduct = testProduct with
        {
            Price = productDTO.Price
        };

        Assert.IsType<NotFoundObjectResult>(result);
        A.CallTo(() => productsRepository.UpdateProductAsync(updatedProduct)).MustNotHaveHappened();
    }
}