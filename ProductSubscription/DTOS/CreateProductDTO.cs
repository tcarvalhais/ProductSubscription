namespace ProductSubscription.DTOS
{
    public record CreateProductDTO
    {
        public required string Name { get; init; }

        public required Guid CreatorUserId { get; init; }

        public required float Price { get; set; }
    }
}