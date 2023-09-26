using System.Text.Json.Serialization;

namespace atprotosharp.Models
{
    public class ServerDescription
    {
        [JsonPropertyName("availableUserDomains")]
        public List<string>? AvailableUserDomains { get; set; }
        [JsonPropertyName("inviteCodeRequired")]
        public bool? InviteCodeRequired { get; set; }
        [JsonPropertyName("links")]
        public Links? Links { get; set; }
    }

    public class Links 
    {
        [JsonPropertyName("privacyPolicy")]
        public string? PrivacyPolicy { get; set; }
        [JsonPropertyName("termsOfService")]
        public string? TermsOfService { get; set; }
    }
}
