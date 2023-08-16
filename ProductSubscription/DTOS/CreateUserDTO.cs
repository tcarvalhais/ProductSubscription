namespace ProductSubscription.DTOS
{
    public record CreateUserDTO
    {
        public required string Name { get; init; }
    }
}