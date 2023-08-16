using ProductSubscription.DTOS;
using ProductSubscription.Models;

namespace ProductSubscription
{
    public static class Extensions
    {
        public static UserDTO AsDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                ListSubscribedUsers = user.ListSubscribedUsers,
                ListFollowers = user.ListFollowers
            };
        }

        public static ProductDTO AsDTO(this Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                CreatorUserId = product.CreatorUserId,
                Price = product.Price
            };
        }
    }
}