using Newtonsoft.Json;

namespace Takerman.Dating.Services.FacebookAuthentication
{
    public class FacebookTokenValidationResponse
    {
        [JsonProperty("data")]
        public FacebookTokenValidationData Data { get; set; }
    }
    public class FacebookTokenValidationData
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("application")]
        public string Application { get; set; } = string.Empty;

        [JsonProperty("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("scopes")]
        public string[] Scopes { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; } = string.Empty;
    }

    public class Metadata
    {
        [JsonProperty("auth_type")]
        public string AuthType { get; set; } = string.Empty;
    }
}
