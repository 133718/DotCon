using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class DayCollection : IJsonOnDeserialized
    {
        [JsonInclude]
        [JsonPropertyName("weekStart")]
        public DateTime StartDate { get; private set; }
        [JsonInclude]
        [JsonPropertyName("weekEnd")]
        public DateTime EndDate { get; private set; }
        [JsonInclude]
        [JsonPropertyName("weekDays")]
        public List<Day> Days { get; private set; }

        public int Count => Days.Count;

        void IJsonOnDeserialized.OnDeserialized()
        {
            if(Days == null)
                Days = new List<Day>();
        }
    }
}
