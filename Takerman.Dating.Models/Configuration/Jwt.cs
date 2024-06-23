﻿namespace Takerman.Dating.Models.Configuration
{
    public class Jwt
    {
        public string Secret { get; set; } = string.Empty;
        
        public string ValidIssuer { get; set; } = string.Empty;

        public string ValidAudience { get; set; } = string.Empty;

        public string DurationInMinutes { get; set; } = string.Empty;

        public string RefreshTokenExpiration { get; set; } = string.Empty;
    }
}
