namespace ProductSubscription.DTOS
{
    public record ProductDTO
    {
        public Guid Id { get; init; }

        public required string Name { get; init; }

        public required Guid CreatorUserId { get; init; }

        public required double Price { get; set; }
    }
}