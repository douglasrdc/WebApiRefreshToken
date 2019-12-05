using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication6
{
    public static class IHttpContextAccessorExtension
    {
        public static string CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.Claims.Where(c => c.Type ==  JwtRegisteredClaimNames.Jti).FirstOrDefault().Value;
            

            return stringId;
        }

        public static string CurrentUserEmail(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.Claims.Where(c=>c.Type== ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            
            return stringId;
        }
    }
}
