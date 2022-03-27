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

        public Lesson this[int index]
        {
            get
            {
                if (index < 0 || index >= Lessons.Count)
                    throw new IndexOutOfRangeException();
                return Lessons[index];
            }
        }

        public Lesson GetLesson(int number) => Lessons.Find(lesson => lesson.Number == number);

        void IJsonOnDeserialized.OnDeserialized()
        {
            if (Lessons == null)
                Lessons = new List<Lesson>();
        }
    }
}
