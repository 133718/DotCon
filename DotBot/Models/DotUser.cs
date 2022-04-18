using SgoApi.Diary;
using System;

namespace DotBot.Models
{
    internal class DotUser
    {
        public DateTime Date { get; private set; }

        public Day Day { get; set; }

        public DotUser()
        {
            var date = DateTime.Now;
            if(date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(1);
            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);
            Date = date;
        }

        public void Add()
        {
            Date = Date.AddDays(1);
            if (Date.DayOfWeek == DayOfWeek.Saturday)
                Date = Date.AddDays(1);
            if (Date.DayOfWeek == DayOfWeek.Sunday)
                Date = Date.AddDays(1);
        }

        public void Sub()
        {
            Date = Date.AddDays(-1);
            if (Date.DayOfWeek == DayOfWeek.Sunday)
                Date = Date.AddDays(-1);
            if (Date.DayOfWeek == DayOfWeek.Saturday)
                Date = Date.AddDays(-1);
        }

        public void Set(int day, int month)
        {
            Date =  new DateTime(Date.Year, month, day);
        }
    }

    enum MessageType
    {
        LessonMessage,
        DayMessage,
        ErorrMessage,
        OtherMessage
    }
        
}
