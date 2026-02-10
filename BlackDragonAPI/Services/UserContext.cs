using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BlackDragonAPI.Services
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        public string? UserId =>
            Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? Principal?.FindFirstValue("sub"); // fallback for some JWT setups

        public string? Email =>
            Principal?.FindFirstValue(ClaimTypes.Email)
            ?? Principal?.FindFirstValue("email");

        public IReadOnlyCollection<string> Roles =>
            Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).Distinct().ToArray()
            ?? Array.Empty<string>();

        public bool IsInRole(string role) => Principal?.IsInRole(role) == true;

        public Claim? FindClaim(string claimType) => Principal?.FindFirst(claimType);
    }
}
