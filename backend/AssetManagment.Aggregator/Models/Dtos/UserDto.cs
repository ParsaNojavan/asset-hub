namespace AssetManagment.Aggregator.Models.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ImgUrl { get; set; }
    }
}
