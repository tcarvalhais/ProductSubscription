using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Guid>> GetAllSubscribedUsersAsync(Guid userId);
        Task<IEnumerable<Guid>> GetAllFollowersAsync(Guid userId);
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task SubscribeUser(Guid userId, Guid subscribedUserId);
    }
}