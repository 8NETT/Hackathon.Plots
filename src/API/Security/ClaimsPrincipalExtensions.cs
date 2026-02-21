namespace API.Security;

internal static class ClaimsPrincipalExtensions
{
    public static Guid? GetAuthenticatedUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userIdClaim is null)
            return null;
        if (!Guid.TryParse(userIdClaim, out var authenticatedUserId))
            return null;

        return authenticatedUserId;
    }
}
