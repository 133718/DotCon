using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SgoApi.Diary
{
    public class Assignment : IJsonOnDeserialized
    {
        [JsonInclude]
        [JsonPropertyName("id")]
        public int Id { get; private set; }
        [JsonInclude]
        [JsonPropertyName("typeId")]
        public int TypeId { get; private set; }
        [JsonInclude]
        [JsonPropertyName("assignmentName")]
        public string FirstDescription { get; private set; }
        [JsonInclude]
        [JsonPropertyName("dueDate")]
        public DateTime Date { get; private set; }
        public Nullable<int> Mark { get; private set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        void IJsonOnDeserialized.OnDeserialized()
        {
            if (!ExtensionData.ContainsKey("mark"))
                return;
            var mark = ExtensionData["mark"];
            Mark = mark.GetProperty("mark").GetInt32();
        }
    }
}
