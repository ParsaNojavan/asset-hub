namespace Storage.Application.DTOs
{
    public class UploadFileResultDto
    {
        public string FileName { get; set; } = null!;
        public string Path { get; set; } = null!;
        public long Size { get; set; }
    }
}