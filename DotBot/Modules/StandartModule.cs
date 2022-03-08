using Discord.Commands;
using DotBot.Services;
using System.Threading.Tasks;

namespace DotBot.Modules
{
    internal class StandartModule : ModuleBase<SocketCommandContext>
    {
        private StopService stopService;

        public StandartModule(StopService stop)
        {
            stopService = stop;
        }

        [Command("dodoco")]
        public Task Dodoco() => ReplyAsync("Dodoco? Dodoco~, where are you~?");

        [Command("exit")]
        public async Task Exit()
        {
            await ReplyAsync("Bot stoped");
            await Context.Client.StopAsync();
            stopService.StopMainTask();
        }
    }
}
