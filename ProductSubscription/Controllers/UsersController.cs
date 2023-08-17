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
        private readonly IUsersRepository usersRepository;
        private readonly IProductsRepository productsRepository;

        public UsersController(IUsersRepository usersRepository, IProductsRepository productsRepository)
        {
            this.usersRepository = usersRepository;
            this.productsRepository = productsRepository;
        }

        // Get all users
        [HttpGet("getAllUsers")]
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = (await usersRepository.GetAllUsersAsync()).Select(user => user.AsDTO());
            return users;
        }

        // Get user by id
        [HttpGet("getUser/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserAsync(Guid userId)
        {
            var user = await usersRepository.GetUserAsync(userId);
            if (user is null)
            {
                return NotFound("User not found");
            }

            return user.AsDTO();
        }

        // Get all subscribed users from that user, given its id
        [HttpGet("getAllSubscribedUsers/{userId}")]
        public async Task<IEnumerable<User>> GetAllSubscribedUsersAsync(Guid userId)
        {
            var users = await usersRepository.GetAllSubscribedUsersAsync(userId);
            return users;
        }

        // Get all subscribers from that user, given its id
        [HttpGet("getAllFollowers/{userId}")]
        public async Task<IEnumerable<User>> GetAllFollowersAsync(Guid userId)
        {
            var users = await usersRepository.GetAllFollowersAsync(userId);
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

            await usersRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserAsync), new { userId = user.Id }, user.AsDTO());
        }

        // Delete a user
        [HttpDelete("deleteUser/{userId}")]
        public async Task<ActionResult> DeleteUserAsync(Guid userId)
        {
            var existingUser = await usersRepository.GetUserAsync(userId);
            if (existingUser is null)
            {
                return NotFound("User not found");
            }

            await usersRepository.UnsubscribeAllUsersAsync(userId);
            await usersRepository.RemoveFollowersAsync(userId);
            await productsRepository.DeleteAllProductsFromUserAsync(userId);
            await usersRepository.DeleteUserAsync(userId);

            return NoContent();
        }

        // Subscribe to an user
        [HttpPut("subscribeUser/{userId}/{subscribedUserId}")]
        public async Task<ActionResult> SubscribeUser(Guid userId, Guid subscribedUserId)
        {
            var existingUser = await usersRepository.GetUserAsync(userId);
            var existingSubscribedUser = await usersRepository.GetUserAsync(subscribedUserId);
            var listSubscribedUsers = (await usersRepository.GetAllSubscribedUsersAsync(userId)).Select(user => user.Id);

            if (existingUser is null || existingSubscribedUser is null)
            {
                return NotFound("User not found");
            }

            if (listSubscribedUsers.Contains(subscribedUserId))
            {
                return Conflict("User already subscribed");
            }

            await usersRepository.SubscribeUserAsync(userId, subscribedUserId);
            return NoContent();
        }

        // Subscribe to an user
        [HttpPut("unsubscribeUser/{userId}/{subscribedUserId}")]
        public async Task<ActionResult> UnsubscribeUser(Guid userId, Guid subscribedUserId)
        {
            var existingUser = await usersRepository.GetUserAsync(userId);
            var existingSubscribedUser = await usersRepository.GetUserAsync(subscribedUserId);
            var listSubscribedUsers = (await usersRepository.GetAllSubscribedUsersAsync(userId)).Select(user => user.Id);

            if (existingUser is null || existingSubscribedUser is null || !listSubscribedUsers.Contains(subscribedUserId))
            {
                return NotFound("User not found or not subscribed");
            }

            await usersRepository.UnsubscribeUserAsync(userId, subscribedUserId);
            return NoContent();
        }
    }
}