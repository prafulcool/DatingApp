using System.Security.Claims;

namespace API.Extensions;

public static class ClaimPrincipleExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
        throw new Exception("cannot get username from token");
        return username;
    }
}