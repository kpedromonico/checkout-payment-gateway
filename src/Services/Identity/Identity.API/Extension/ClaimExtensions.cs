using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Identity.API.Extension
{
    public static class ClaimExtensions
    {
        public static DateTime? GetExpiryDateClaim(this ClaimsPrincipal principal) 
        {
            var expiryStr = principal.Claims
                .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?
                .Value;

            if(long.TryParse(expiryStr, out long epochSeconds))
            {
                return DateTime.UnixEpoch.AddSeconds(epochSeconds);
            }
            else
            {
                return null;
            }
        }

        public static string GetJtiClaim(this ClaimsPrincipal principal)
        {
            return principal.Claims
                .SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?
                .Value;            
        }
    }
}
