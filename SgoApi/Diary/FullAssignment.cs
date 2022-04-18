using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class FullAssignment
    {
        [JsonPropertyName("assignmentName")]
        public string FirstDescriprition { get; set; }
        [JsonPropertyName("teachers")]
        public Teacher[] Teachers { get; set; }
        [JsonPropertyName("weight")]
        public int Weight { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("attachments")]
        public SgoAttachment[] Attachments { get; set; }
    }

    public class Teacher
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class SgoAttachment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("originalFileName")]
        public string OriginalFileName { get; set; }
        [JsonPropertyName("description")]
        public object Description { get; set; }
    }
}



