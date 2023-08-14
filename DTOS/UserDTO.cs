using ProductSubscription.Models;

namespace ProductSubscription.DTOS
{
    public record UserDTO
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public required List<Guid> ListSubscribedUsers { get; set; }
        public required List<Guid> ListFollowers { get; set; }
    }
}