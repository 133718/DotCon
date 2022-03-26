using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class Day
    {
        public DateTime Date { get; }
        public List<object> Lessons { get; }

        public int Count => Lessons.Count;

        [JsonConstructor]
        internal Day(DateTime date, List<object> lessons)
        {
            Date = date;
            Lessons = lessons ?? new List<object>();
        }
    }
}
