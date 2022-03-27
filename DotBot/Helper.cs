using System;

namespace DotBot
{
    internal static class Helper
    {
        public static string GetMonthName(int number) =>
            number switch
            {
                1 => "января",
                2 => "февраля",
                3 => "марта",
                4 => "апреля",
                5 => "мая",
                6 => "июня",
                7 => "июля",
                8 => "августа",
                9 => "сеньября",
                10 => "октября",
                11 => "нояюря",
                12 => "декабря",
                _ => throw new ArgumentException()
            };
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
    }
}
