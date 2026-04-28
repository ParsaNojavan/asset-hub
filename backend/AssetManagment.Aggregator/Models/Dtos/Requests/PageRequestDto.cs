namespace AssetManagment.Aggregator.Models.Dtos.Requests
{
    public class PageRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
