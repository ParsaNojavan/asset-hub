namespace Storage.API.Extensions
{
    public static class StoragePathHelper
    {
        public static string GetUserRoot(Guid userId)
        {
            return Path.Combine("users",userId.ToString());
        }

        public static string CombineUserPath(Guid userId, string relativePath)
        {
            var userRoot = GetUserRoot(userId);
            return Path.Combine(userRoot,relativePath);
        }
    }
}
