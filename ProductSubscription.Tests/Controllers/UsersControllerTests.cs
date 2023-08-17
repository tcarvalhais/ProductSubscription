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
    public async void UsersController_GetAllSubscribedUsersAsync_ReturnSuccess()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var subscribedTestUser = new User { Id = subscribedUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var listSubscribedUsers = new List<User> { subscribedTestUser };
        A.CallTo(() => usersRepository.GetAllSubscribedUsersAsync(userId)).Returns(listSubscribedUsers);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.GetAllSubscribedUsersAsync(userId);

        // Assert
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(result);
        Assert.Equal(listSubscribedUsers.Count, users.Count());
    }

    [Fact]
    public async void UsersController_GetAllFollowersAsync_ReturnSuccess()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var followerUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var followerTestUser = new User { Id = followerUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };

        var listFollowers = new List<User> { followerTestUser };
        A.CallTo(() => usersRepository.GetAllFollowersAsync(userId)).Returns(listFollowers);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.GetAllFollowersAsync(userId);

        // Assert
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(result);
        Assert.Equal(listFollowers.Count, users.Count());
    }

    [Fact]
    public async Task UsersController_GetUserAsync_UserExists()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
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
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultUser.Result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UsersController_CreateUserAsync_UserCreated()
    {
        // Arrange
        var testUserDTO = new CreateUserDTO { Name = "Margrethe Ingrid" };

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.CreateUserAsync(testUserDTO);

        // Assert
        A.CallTo(() => usersRepository.CreateUserAsync(A<User>.Ignored)).MustHaveHappenedOnceExactly();
        Assert.IsType<CreatedAtActionResult>(result.Result);

        if (result.Result is CreatedAtActionResult createdAtAction)
        {
            Assert.IsType<UserDTO>(createdAtAction.Value);
        }
        else
        {
            Assert.True(false, "Expected a CreatedAtActionResult.");
        }
    }

    [Fact]
    public async Task UsersController_DeleteUserAsync_ExistingUser()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
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
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns((User)null);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.DeleteUserAsync(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UsersController_SubscribeUser_UserSubscribedWithSuccess()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);
        A.CallTo(() => usersRepository.GetAllSubscribedUsersAsync(userId)).Returns(new List<User>());

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var subscribedTestUser = new User { Id = subscribedUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(subscribedUserId)).Returns(subscribedTestUser);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.SubscribeUser(userId, subscribedUserId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UsersController_SubscribeUser_UserNotFound()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns((User)null);

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var subscribedTestUser = new User { Id = subscribedUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(subscribedUserId)).Returns(subscribedTestUser);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.SubscribeUser(userId, subscribedUserId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UsersController_SubscribeUser_SubscribedUserNotFound()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);
        A.CallTo(() => usersRepository.GetAllSubscribedUsersAsync(userId)).Returns(new List<User>());

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        A.CallTo(() => usersRepository.GetUserAsync(subscribedUserId)).Returns((User)null);

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.SubscribeUser(userId, subscribedUserId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UsersController_SubscribeUser_UserAlreadySubscribed()
    {
        // Arrange
        var userId = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f");
        var testUser = new User { Id = userId, Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(userId)).Returns(testUser);

        var subscribedUserId = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd");
        var subscribedTestUser = new User { Id = subscribedUserId, Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() };
        A.CallTo(() => usersRepository.GetUserAsync(subscribedUserId)).Returns(subscribedTestUser);
        A.CallTo(() => usersRepository.GetAllSubscribedUsersAsync(userId)).Returns(new List<User>() { subscribedTestUser });

        // Act
        var controller = new UsersController(usersRepository, productsRepository);
        var result = await controller.SubscribeUser(userId, subscribedUserId);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("User already subscribed", conflictResult.Value);
    }
}