// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Models
{
    public class SgoUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Organization
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class AccountInfo
    {
        [JsonPropertyName("currentOrganization")]
        public Organization CurrentOrganization { get; set; }

        [JsonPropertyName("user")]
        public SgoUser User { get; set; }

        [JsonPropertyName("userRoles")]
        public List<object> UserRoles { get; set; }

        [JsonPropertyName("organizations")]
        public List<Organization> Organizations { get; set; }

        [JsonPropertyName("loginTime")]
        public DateTime LoginTime { get; set; }
    }

    public class UserLogInfo
    {
        [JsonPropertyName("at")]
        public string AuthToken { get; set; }

        [JsonPropertyName("accountInfo")]
        public AccountInfo AccountInfo { get; set; }
    }

}