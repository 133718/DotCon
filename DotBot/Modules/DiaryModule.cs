using Discord.Commands;
using Discord;
using SgoApi;
using System;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using DotBot.Models;
using System.Collections.Generic;
using SgoApi.Diary;
using DotBot.Services;

namespace DotBot.Modules
{
    internal class DiaryModule : ModuleBase<SocketCommandContext>
    {
        readonly SgoClient _client;
        readonly DotUser _user;

        public DiaryModule(SgoClient sgoClient, DotUser user)
        {
            _client = sgoClient;
            _user = user;
        }

        #region Diary
        [Command("diary")]
        public async Task<RuntimeResult> Diary()
        {
            var days = await _client.Diary.GetDay(_user.Date);
            var culture = CultureInfo.GetCultureInfo("ru-RU");
            var embed = new EmbedBuilder() { 
                Title = "Дневник", 
                Description = $"*{days.StartDate.ToString("dddd", culture).FirstCharToUpper()} {days.StartDate.Day} {Helper.GetMonthName(days.StartDate.Month)}*",
                Timestamp = DateTime.Now,
                Color = new Color(0, 255, 255)
            };
            
            if (days.Count > 0)
            {
                var day = days[0];
                _user.Day = day;
                var maxLesson = day[^1].Number;
                for(int i = 1; i <= maxLesson; i++)
                {
                    var lesson = day.GetLesson(i);
                    if (lesson != null)
                    {
                        var field = new EmbedFieldBuilder() { Name = $"> {i} {lesson.Subject}" };
                        var homework = lesson.GetAssignment(3);
                        if (homework != null)
                            field.Value = "```" + (homework.Description.Length > 100 ? string.Concat(homework.Description.AsSpan(0, 98), " …") : homework.Description) + "```";
                        else
                            field.Value = "```нема дз```";
                        embed.AddField(field);
                    }
                    else
                        embed.AddField($"> {i}", "```*Нет урока*```");
                }
            }
            else
                embed.AddField("> Нет уроков", "вообще нема");

            var response = await Context.Channel.SendMessageAsync("", false, embed.Build());
            return ModuleResult.FromSuccess(response);
        }

        [Command("diary")]
        public async Task<RuntimeResult> Diary(string date)
        {
            var data = date.Split( new char[] { '.', '/', ',' } );
            if(data.Length == 2)
            {
                if(int.TryParse(data[0], out int num1) && int.TryParse(data[1], out int num2))
                {
                    return await Diary(num1, num2);
                }
            }
            return null;
        }

        [Command("diary")]
        public async Task<RuntimeResult> Diary(int num1, int num2)
        {
            _user.Set(num1, num2);
            return await Diary();
        }

        [Command("next")]
        public async Task<RuntimeResult> GetNextDay()
        {
            _user.Add();
            return await Diary();
        }

        [Command("prev")]
        public async Task<RuntimeResult> GetPrevDay()
        {
            _user.Sub();
            return await Diary();
        }

        #endregion

        [Command("lesson")]
        public async Task<RuntimeResult> GetLesson(int num)
        {
            if (_user.Day is null || _user.Day.Count == 0)
            {
                var tresponse = await Context.Channel.SendMessageAsync("День не выбран");
                return ModuleResult.FromSuccess(tresponse);
            }   
            
            var lesson = _user.Day.GetLesson(num);

            if (lesson is null)
            {
                var tresponse = await Context.Channel.SendMessageAsync($"Урока под номером {num} нет");
                return ModuleResult.FromSuccess(tresponse);
            }

            var embed = new EmbedBuilder
            {
                Title = $"{lesson.Number} {lesson.Subject}",
                Description = $"{lesson.StartTime} - {lesson.EndTime} в {lesson.Room}",
                Color = new Color(0, 255, 255),
                Timestamp = DateTime.Now                
            };
            //TODO Change field creation

            var attachments = new List<SgoAttachment>();

            foreach(var assignment in lesson.Assignments)
            {
                var fullAssigment = await _client.Diary.GetAssignmentAsync(assignment.Id);
                var s = new StringBuilder();
                s.AppendLine("> Описание");
                s.AppendLine($"```{fullAssigment.FirstDescriprition}```");
                if (!string.IsNullOrEmpty(fullAssigment.Description))
                {
                    s.Append('\n');
                    s.AppendLine("> Подробности");
                    s.AppendLine($"```{fullAssigment.FirstDescriprition}```");
                }
                embed.AddField(Helper.GetAssignmentName(assignment.TypeId), s.ToString());
                attachments.AddRange(fullAssigment.Attachments);
            }
            if (attachments.Count > 0)
            {
                var s = new StringBuilder();
                foreach (var attachment in attachments)
                    s.AppendLine("> " + attachment.Name + '\n');
                embed.AddField("Файлы", s.ToString());
            }

            var response = await Context.Channel.SendMessageAsync("", false, embed.Build());
            return ModuleResult.FromSuccess(response);
        }
    }
} 
