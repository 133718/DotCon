using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DotBot.Services
{
    internal class ServerNormolizeService
    {
        private readonly DiscordSocketClient _client;

        public ServerNormolizeService(DiscordSocketClient client) 
        {
            _client = client;
        }


    }
}
