using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SgoApi.Models
{
    internal class UserLogInfo
    {
        [JsonProperty("at")]
        public string authToken;

        public string name;
        public string id;

        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            var AccountInfo = _additionalData["accountInfo"]["user"];

            name = AccountInfo["name"].ToString();
            id = AccountInfo["id"].ToString();
        }
    }
}
