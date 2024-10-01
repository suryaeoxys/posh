using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Utility.JwtUtils
{
    public static class JwtAuthenticationDefaults
    {
        public const string AuthenticationScheme = "JWT";
        public const string HeaderName = "Authorization";
        public const string BearerPrefix = "Bearer";

    }
}
