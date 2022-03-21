using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotBot
{
    internal class JsonConfig
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ulong DiscordId { get; set; }
    }
}
