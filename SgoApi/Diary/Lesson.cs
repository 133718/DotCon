using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class Lesson
    {
        public DateTime Day { get; }
        public int Number { get; }
        public string Room { get; }
        public string StartTime { get; }
        public string EndTime { get; }
        public string SubjectName { get; }
        public List<object> Assignments { get; }
        
        public int Count => Assignments.Count;

        [JsonConstructor]
        internal Lesson (DateTime day, int number, string room, string startTime, string endTime, string subjectName, List<object> assignments)
        {
            Day = day;
            Number = number;
            Room = room;
            StartTime = startTime;
            EndTime = endTime;
            SubjectName = subjectName;
            Assignments = assignments ?? new List<object>();
        }
    }
}
