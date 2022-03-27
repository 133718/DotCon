using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class Lesson : IJsonOnDeserialized
    {
        [JsonInclude]
        [JsonPropertyName("day")]
        public DateTime Date { get; private set; }
        [JsonInclude]
        [JsonPropertyName("number")]
        public int Number { get; private set; }
        [JsonInclude]
        [JsonPropertyName("room")]
        public string Room { get; private set; }
        [JsonInclude]
        [JsonPropertyName("startTime")]
        public string StartTime { get; private set; }
        [JsonInclude]
        [JsonPropertyName("endTime")]
        public string EndTime { get; private set; }
        [JsonInclude]
        [JsonPropertyName("subjectName")]
        public string Subject { get; private set; }
        [JsonInclude]
        [JsonPropertyName("assignments")]
        public List<Assignment> Assignments { get; private set; }
        
        public int Count => Assignments.Count;

        public Assignment this[int index]
        {
            get
            {
                if (index < 0 || index >= Assignments.Count)
                    throw new IndexOutOfRangeException();
                return Assignments[index];
            }
        }

        public Assignment GetAssignment(int num) => Assignments.Find(assignment => assignment.TypeId == num);

        void IJsonOnDeserialized.OnDeserialized()
        {
            if(Assignments == null)
                Assignments = new List<Assignment>();
        }
    }
}
