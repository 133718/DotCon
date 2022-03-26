using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SgoApi.Diary
{
    public class DayCollection
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public List<Day> Days { get; }

        public int Count => Days.Count;

        [JsonConstructor]
        internal DayCollection(DateTime weekStart, DateTime weekEnd, List<Day> weekDays)
        {
            StartDate = weekStart;
            EndDate = weekEnd;
            Days = weekDays ?? new List<Day>();
        }
    }
}
