using System;

namespace Identity.API.Configurations
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}