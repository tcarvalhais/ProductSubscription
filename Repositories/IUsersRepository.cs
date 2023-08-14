using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllSubscribedUsersAsync(Guid userId);
        Task<IEnumerable<User>> GetAllFollowersAsync(Guid userId);
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task SubscribeUserAsync(Guid userId, Guid subscribedUserId);
        Task UnsubscribeUserAsync(Guid userId, Guid subscribedUserId);
        Task UnsubscribeAllUsersAsync(Guid userId);
        Task RemoveFollowersAsync(Guid userId);
    }
}