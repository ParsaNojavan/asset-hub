namespace UserService.Application.DTOs.Responses
{
    public class UserSearchDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImgUrl { get; set; }
    }
}