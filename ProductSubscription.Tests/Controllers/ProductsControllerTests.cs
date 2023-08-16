using Microsoft.AspNetCore.Mvc;
using Xunit;
using FakeItEasy;
using ProductSubscription.Repositories;
using ProductSubscription.DTOS;
using ProductSubscription.Controllers;
using ProductSubscription.Models;

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
}