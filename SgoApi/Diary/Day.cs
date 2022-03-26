using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class Day : IJsonOnDeserialized
    {
        [JsonInclude]
        [JsonPropertyName("date")]
        public DateTime Date { get; private set; }
        [JsonInclude]
        [JsonPropertyName("lessons")]
        public List<Lesson> Lessons { get; private set; }

        public int Count => Lessons.Count;

        void IJsonOnDeserialized.OnDeserialized()
        {
            if (Lessons == null)
                Lessons = new List<Lesson>();
        }
    }
}
