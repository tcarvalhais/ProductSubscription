using System.ComponentModel.DataAnnotations;

namespace ProductSubscription.Models
{
    public class User
    {
        public Guid Id { get; init; }

        [MinLength(3)]
        public required string Name { get; init; }

        public required List<Guid> ListSubscribedUsers { get; set; }

        public required List<Guid> ListFollowers { get; set; }
    }
}