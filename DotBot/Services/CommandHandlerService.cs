using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotBot
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
			await _commands.AddModulesAsync(
				assembly: Assembly.GetEntryAssembly(),
				services: _services);
			//_client.MessageReceived += HandleCommandAsync;
		}
	}
}
