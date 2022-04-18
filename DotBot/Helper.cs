using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;

namespace DotBot
{
    internal static class Helper
    {
        static readonly Dictionary<int, string> types = new()
        {
            { 1, "Практическая работа" },
            { 2, "Тематическая работа" },
            { 3, "Домашнее задание" },
            { 4, "Контрольная работа" },
            { 5, "Самостоятельная работа" },
            { 6, "Лабораторная работа" },
            { 7, "Проект" },
            { 8, "Диктант" },
            { 9, "Реферат" },
            { 10, "Ответ на уроке" },
            { 11, "Сочинение" },
            { 12, "Изложение" },
            { 13, "Зачёт" },
            { 14, "Тестирование" },
            { 16, "Диагностическая контрольная работа" },
            { 17, "Домашняя контрольная работа" },
            { 18, "Дистанционное обучение" },
            { 19, "Словарный диктант" },
            { 20, "Проверочная работа" },
            { 21, "Аудирование" },
            { 22, "Контурные карты" },
            { 23, "Эссе" },
            { 24, "Устный счёт" },
            { 25, "Диктант с грамматическим зад." },
            { 26, "Наизусть" },
            { 27, "Термины" },
            { 28, "Ведение тетради" },
            { 29, "Индивидуальная карточка" },
            { 30, "Творческое задание" },
            { 31, "Практическое упражнение" },
            { 32, "Урок-игра" },
            { 33, "Урок-экскурсия" },
            { 34, "Монологическое высказывание" },
            { 35, "Диалогическое высказывание" }
        };

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
                _ => throw new ArgumentException("Wrong month number")
            };

        public static string GetAssignmentName(int number)
        {
            if(types.TryGetValue(number, out var type))
            {
                return type;
            }
            else
            {
                return "Обнови базу";
            }
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
    }

    internal class ModuleResult : RuntimeResult
    {
        public RestUserMessage ctx;

        public ModuleResult(CommandError? error, string reason, RestUserMessage ctx) : base(error, reason)
        {
            this.ctx = ctx;
        }
        public static ModuleResult FromError(string reason) =>
            new(CommandError.Unsuccessful, reason, null);
        public static ModuleResult FromSuccess(RestUserMessage ctx, string reason = null) =>
            new(null, reason, ctx);
    }
}
