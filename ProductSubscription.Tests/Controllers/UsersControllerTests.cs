using FakeItEasy;
using ProductSubscription.Repositories;
using ProductSubscription.DTOS;
using ProductSubscription.Controllers;
using ProductSubscription.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductSubscription.Tests.Controllers;

public class UsersControllerTests
{
    private readonly IProductsRepository productsRepository;
    private readonly IUsersRepository usersRepository;

    public UsersControllerTests()
    {
        productsRepository = A.Fake<IProductsRepository>();
        usersRepository = A.Fake<IUsersRepository>();
    }

    [Fact]
    public async void UsersController_GetAllUsersAsync_ReturnSuccess()
    {
        // Arrange
        var testUser = new User { Id = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetAllUsersAsync()).Returns(new List<User> { testUser });

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.GetAllUsersAsync();

        // Assert
        var resultUsers = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(result);
        Assert.Single(resultUsers);
    }

    [Fact]
    public async Task UsersController_GetUserAsync_UserExists()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.GetUserAsync(userId);

        // Assert
        var resultUser = Assert.IsAssignableFrom<ActionResult<UserDTO>>(result);
        Assert.Equal(userId, resultUser.Value.Id);
    }

    [Fact]
    public async Task UsersController_GetUserAsync_UserNotFound()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns((User)null);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.GetUserAsync(userId);

        // Assert
        var resultUser = Assert.IsAssignableFrom<ActionResult<UserDTO>>(result);
        Assert.IsType<NotFoundResult>(resultUser.Result);
    }

    [Fact]
    public async Task UsersController_DeleteUserAsync_ExistingUser()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.DeleteUserAsync(userId);

        // Assert
        A.CallTo(() => usersRepository.DeleteUserAsync(userId)).MustHaveHappenedOnceExactly();
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UsersController_DeleteUserAsync_UserNotFound()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns((User)null);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.DeleteUserAsync(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}