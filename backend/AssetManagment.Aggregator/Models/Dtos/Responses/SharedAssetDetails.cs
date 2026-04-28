namespace AssetManagment.Aggregator.Models.Dtos.Responses
{
    public class SharedAssetDetails
    {
        public AssetDetailsDto Asset { get; set; }
        public IEnumerable<UserDto> Recivers { get; set; }
    }
}
