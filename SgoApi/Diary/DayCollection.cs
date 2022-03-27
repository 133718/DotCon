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

        public Day this[int index]
        {
            get
            {
                if (index < 0 || index >= Days.Count)
                    throw new IndexOutOfRangeException();
                return Days[index];
            }
        }

        void IJsonOnDeserialized.OnDeserialized()
        {
            if(Days == null)
                Days = new List<Day>();
        }
    }
}
