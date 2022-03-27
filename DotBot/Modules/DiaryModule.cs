using Discord.Commands;
using Discord;
using SgoApi;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DotBot.Modules
{
    internal class DiaryModule : ModuleBase<SocketCommandContext>
    {
        readonly SgoClient _client;

        public DiaryModule(SgoClient sgoClient)
        {
            _client = sgoClient;
        }

        [Command("diary")]
        public async Task Diary()
        {
            var days = await _client.Diary.GetDay(new DateTime(2022, 03, 14));
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            var embed = new EmbedBuilder() { 
                Title = "Дневник", 
                Description = $"*{days.StartDate.ToString("dddd", culture).FirstCharToUpper()} {days.StartDate.Day} {Helper.GetMonthName(days.StartDate.Month)}*",
                Timestamp = DateTime.Now,
                Color =new Color(0, 255, 255)
            };
            var day = days[0];
            if (day.Count > 0)
            {
                var maxLesson = day[^1].Number;
                for(int i = 1; i <= maxLesson; i++)
                {
                    var lesson = day.GetLesson(i);
                    if (lesson != null)
                    {
                        var field = new EmbedFieldBuilder() { Name = $"> {i} {lesson.Subject}" };
                        var homework = lesson.GetAssignment(3);
                        if (homework != null)
                            field.Value = $"```{homework.Description }```";
                        embed.AddField(field);
                    }
                    else
                        embed.AddField($"> {i}", "`*Нет урока*`");
                }
            }
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
} 
