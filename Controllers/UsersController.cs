using System.Collections;
using Microsoft.AspNetCore.Mvc;
using ProductSubscription.DTOS;
using ProductSubscription.Models;
using ProductSubscription.Repositories;

namespace ProductSubscription.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository repository;

        public UsersController(IUsersRepository repository)
        {
            this.repository = repository;
        }

        // Get all users
        [HttpGet("getAllUsers")]
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = (await repository.GetAllUsersAsync()).Select(user => user.AsDTO());
            return users;
        }

        // Get user by id
        [HttpGet("getUser/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserAsync(Guid userId)
        {
            var user = await repository.GetUserAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            return user.AsDTO();
        }

        // Get all subscribed users from that user, given its id
        [HttpGet("getAllSubscribedUsers/{userId}")]
        public async Task<IEnumerable<Guid>> GetAllSubscribedUsersAsync(Guid userId)
        {
            var users = await repository.GetAllSubscribedUsersAsync(userId);
            return users;
        }

        // Get all subscribers from that user, given its id
        [HttpGet("getAllFollowers/{userId}")]
        public async Task<IEnumerable<Guid>> GetAllFollowersAsync(Guid userId)
        {
            var users = await repository.GetAllFollowersAsync(userId);
            return users;
        }

        // Create a new user
        [HttpPost("createUser")]
        public async Task<ActionResult<UserDTO>> CreateUserAsync(CreateUserDTO userDTO)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                Name = userDTO.Name,
                ListSubscribedUsers = new List<Guid>(),
                ListFollowers = new List<Guid>()
            };

            await repository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user.AsDTO());
        }

        // Delete a user
        [HttpDelete("deleteUser/{userId}")]
        public async Task<ActionResult> DeleteUserAsync(Guid userId)
        {
            var existingUser = await repository.GetUserAsync(userId);
            if (existingUser is null)
            {
                return NotFound();
            }

            await repository.DeleteUserAsync(userId);
            return NoContent();
        }

        // Subscribe to an user
        [HttpPut("subscribeUser/{userId}/{subscribedUserId}")]
        public async Task<ActionResult> SubscribeUser(Guid userId, Guid subscribedUserId)
        {
            var existingUser = await repository.GetUserAsync(userId);
            var existingSubscribedUser = await repository.GetUserAsync(subscribedUserId);

            if (existingUser is null || existingSubscribedUser is null)
            {
                return NotFound();
            }

            await repository.SubscribeUser(userId, subscribedUserId);
            return NoContent();
        }
    }
}