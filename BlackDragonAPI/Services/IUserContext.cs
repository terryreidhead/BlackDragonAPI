using System.Security.Claims;

namespace BlackDragonAPI.Services
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        string? UserId { get; }
        string? Email { get; }

        IReadOnlyCollection<string> Roles { get; }

        bool IsInRole(string role);
        Claim? FindClaim(string claimType);
    }
}
