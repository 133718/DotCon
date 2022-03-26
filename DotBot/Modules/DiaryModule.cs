using Discord.Commands;
using DotBot.Services;
using SgoApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotBot.Modules
{
    internal class DiaryModule : ModuleBase<SocketCommandContext>
    {
        SgoClient _client;

        public DiaryModule(SgoClient sgoClient)
        {
            _client = sgoClient;
        }

        [Command("diary")]
        public async Task Diary()
        {
            await _client.Diary.GetDays(DateTime.Now, DateTime.Now);
        }
    }
}
