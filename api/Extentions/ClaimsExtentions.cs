using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            return user.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();
        }
        public static string? GetTokenType(this ClaimsPrincipal user)
        {
            return user.FindFirst("TokenType")?.Value;
        }

        public static bool IsAccessToken(this ClaimsPrincipal user)
        {
            var type = user.GetTokenType();
            if (type == null || type != "AccessToken")
            {
                return false;
            }
            return true;
        }
    }
}
