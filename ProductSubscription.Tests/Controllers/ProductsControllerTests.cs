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
        A.CallTo(() => productsRepository.GetAllProductsAsync()).Returns(new List<ProductDTO> { testProduct.AsDTO() });

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
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct.AsDTO());

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
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((ProductDTO)null);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.GetProductAsync(productId);

        // Assert
        var resultProduct = Assert.IsAssignableFrom<ActionResult<ProductDTO>>(result);
        Assert.IsType<NotFoundResult>(resultProduct.Result);
    }

    [Fact]
    public async Task ProductsController_DeleteProductAsync_ExistingProduct()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct.AsDTO());

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
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((ProductDTO)null);

        // Act
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.DeleteProductAsync(productId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task ProductsController_UpdateProductAsync_ExistingProduct()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns(testProduct.AsDTO());

        // Act
        var productDTO = new UpdateProductDTO { Price = 10 };
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.UpdateProductAsync(productId, productDTO);

        // Assert
        A.CallTo(() => productsRepository.UpdateProductAsync(productId, productDTO)).MustHaveHappenedOnceExactly();
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ProductsController_UpdateProductAsync_ProductNotFound()
    {
        // Arrange
        var productId = new Guid("e3dd1eb9-e7f8-4e08-9505-39397b470204");
        var testProduct = new Product { Id = productId, Name = "Royal Copenhagen Dinnerware", CreatorUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Price = 674.99 };
        A.CallTo(() => productsRepository.GetProductAsync(productId)).Returns((ProductDTO)null);

        // Act
        var productDTO = new UpdateProductDTO { Price = 10 };
        var controller = new ProductsController(productsRepository, usersRepository);
        var result = await controller.UpdateProductAsync(productId, productDTO);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        A.CallTo(() => productsRepository.UpdateProductAsync(productId, productDTO)).MustNotHaveHappened();
    }
}