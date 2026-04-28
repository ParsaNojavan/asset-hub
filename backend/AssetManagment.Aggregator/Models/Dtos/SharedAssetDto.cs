namespace AssetManagment.Aggregator.Models.Dtos
{
    public class SharedAssetDto
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }

        public Guid ResourceId { get; set; }

        public List<SharedUserDto>? Users { get; set; }
    }
}
