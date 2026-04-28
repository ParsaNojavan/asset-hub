using System.Security.Claims;

namespace SharedKernel.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("UserId claim not found")
                );
        }
    }
}
