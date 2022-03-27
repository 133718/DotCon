using Discord.Commands;
using Discord.WebSocket;
using DotBot.Models;
using DotBot.Modules;
using System;
using System.Threading.Tasks;

namespace DotBot.Services
{
    internal class CommandHandlerService
    {
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;
        private readonly JsonConfig _jsonConfig;

		public CommandHandlerService(IServiceProvider services, CommandService commands, DiscordSocketClient client, JsonConfig config)
		{
			_commands = commands;
			_services = services;
			_client = client;
            _jsonConfig = config;
		}

		public async Task InitializeAsync()
		{
            await _commands.AddModuleAsync<StandartModule>(_services);
            await _commands.AddModuleAsync<DiaryModule>(_services);
			_client.MessageReceived += HandleCommandAsync;
		}

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            if (message.Author.Id != _jsonConfig.DiscordId) return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
