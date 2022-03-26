using Discord.Commands;
using System.Threading.Tasks;

namespace DotBot.Modules
{
    internal class StandartModule : ModuleBase<SocketCommandContext>
    {
        [Command("dodoco")]
        public Task Dodoco() => ReplyAsync("Dodoco? Dodoco~, where are you~?");
    }
}
