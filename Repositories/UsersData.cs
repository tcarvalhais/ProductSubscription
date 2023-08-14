using ProductSubscription.Models;

namespace ProductSubscription.Repositories
{
    public class UsersData : IUsersRepository
    {
        private List<User> users = new List<User>()
        {
            new User { Id = new Guid("92a1cd49-87fc-4618-a084-022a9b65366f"), Name = "Mette Frederiksen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() },
            new User { Id = new Guid("e9b232ae-076a-48d5-b3e0-ecabbea5d8cd"), Name = "Margrethe Ingrid", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() },
            new User { Id = new Guid("d5a6ff7b-d04f-4ec1-be19-f4a3e3cb605c"), Name = "Mads Mikkelsen", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() },
            new User { Id = new Guid("022cd49d-6e41-49f3-9644-dbbfadd2abe8"), Name = "Nikolaj Waldau", ListSubscribedUsers = new List<Guid>(), ListFollowers = new List<Guid>() }
        };

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = users.Where(user => user.Id == id).SingleOrDefault();
            return await Task.FromResult(user);
        }

        public async Task<IEnumerable<Guid>> GetAllSubscribedUsersAsync(Guid id)
        {
            List<Guid> subscribedUsers = new List<Guid>();

            var user = users.Where(user => user.Id == id).SingleOrDefault();
            if (user is not null)
            {
                subscribedUsers = user.ListSubscribedUsers;
            }

            return await Task.FromResult(subscribedUsers);
        }

        public async Task<IEnumerable<Guid>> GetAllFollowersAsync(Guid id)
        {
            IEnumerable<Guid> followers = new List<Guid>();

            var user = users.Where(user => user.Id == id).SingleOrDefault();
            if (user is not null)
            {
                followers = user.ListFollowers;
            }

            return await Task.FromResult(followers);
        }

        public async Task CreateUserAsync(User user)
        {
            users.Add(user);
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var index = users.FindIndex(existingUser => existingUser.Id == id);
            users.RemoveAt(index);

            await Task.CompletedTask;
        }

        public async Task SubscribeUser(Guid userId, Guid subscribedUserId)
        {
            var subscribedUserIndex = users.FindIndex(existingUser => existingUser.Id == subscribedUserId);
            users[subscribedUserIndex].ListFollowers.Add(userId);

            var userIndex = users.FindIndex(existingUser => existingUser.Id == userId);
            users[userIndex].ListSubscribedUsers.Add(subscribedUserId);

            await Task.CompletedTask;
        }
    }
}