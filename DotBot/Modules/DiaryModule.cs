using Discord.Commands;
using DotBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotBot.Modules
{
    internal class DiaryModule : ModuleBase<SocketCommandContext>
    {
        private SgoConnectionService _connection;

        public DiaryModule(SgoConnectionService service)
        {
            _connection = service;
        }

        public async Task Diary()
        {
            //var day = await _connection.GetDay(DateTime.Now);
            await Context.Channel.SendMessageAsync();
        }
    }
}
