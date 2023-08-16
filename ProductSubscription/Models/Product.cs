using System.ComponentModel.DataAnnotations;

namespace ProductSubscription.Models
{
    public record Product
    {
        public Guid Id { get; init; }

        [MinLength(3)]
        public required string Name { get; init; }

        public required Guid CreatorUserId { get; init; }

        public required double Price { get; set; }
    }
}