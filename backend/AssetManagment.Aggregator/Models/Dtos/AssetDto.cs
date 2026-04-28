namespace AssetManagment.Aggregator.Models.Dtos
{
    public class AssetDto
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public int Size { get; set; }
        public string StorageProvider { get; set; }
        public string StoragePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
