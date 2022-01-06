using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Util
{
    public static class UserUtil
    {
        public static string GetId(this ClaimsPrincipal principal)
        {
            var res = principal.Claims.FirstOrDefault<Claim>(e => e.Type == ClaimTypes.NameIdentifier);
            if (res == null)
            {
                return null;
            }
            return res?.Value;
        }
    }
}
