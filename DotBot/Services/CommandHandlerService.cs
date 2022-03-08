using Discord.Commands;
using Discord.WebSocket;
using DotBot.Modules;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotBot.Services
{
    internal class CommandHandlerService
    {
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;

		public CommandHandlerService(IServiceProvider services, CommandService commands, DiscordSocketClient client)
		{
			_commands = commands;
			_services = services;
			_client = client;
		}

		public async Task InitializeAsync()
		{
            await _commands.AddModuleAsync<StandartModule>(_services);
			_client.MessageReceived += HandleCommandAsync;
		}

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
