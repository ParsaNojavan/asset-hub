namespace AssetManagment.Aggregator.Models.Dtos.Responses
{
    public class ShareDetails
    {
        public IEnumerable<SharedAssetDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
