using Discord.Commands;
using Discord.WebSocket;
using DotBot.Models;
using DotBot.Modules;
using System;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;

namespace DotBot.Services
{
    internal class CommandHandlerService
    {
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;
        private readonly JsonConfig _jsonConfig;
        ulong lastMessageId = 0;
        ulong channelId = 0;

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
            _commands.CommandExecuted += HandleCommandExecuted;
		}

        private async Task HandleCommandExecuted(Discord.Optional<CommandInfo> commandInfo, ICommandContext ctx, IResult result)
        {
            if (!result.IsSuccess)
                return;

            if (ctx.Channel.Name.ToLower().Contains("diary"))
            {
                if (result is ModuleResult mResult)
                {
                    if(lastMessageId != 0)
                    {
                        await ctx.Channel.DeleteMessageAsync(lastMessageId);
                    }

                    lastMessageId = mResult.ctx.Id;
                    channelId = mResult.ctx.Channel.Id;
                }
            }   
        }

        public void Stop()
        {
            if (lastMessageId != 0)
            {
                IMessageChannel channel = _client.GetChannel(channelId) as ITextChannel;
                channel.DeleteMessageAsync(lastMessageId).Wait();
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message) return;

            int argPos = 0;

            if (message.Author.IsBot)
                return;

            if (!(message.HasCharPrefix(_jsonConfig.Prefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) &&
                !message.Channel.Name.ToLower().Contains("diary"))
                return;

            //if (message.Author.Id != _jsonConfig.DiscordId) return

            var context = new SocketCommandContext(_client, message);

            if (message.Channel.Name.ToLower().Contains("diary"))
            {
                await message.DeleteAsync();
            }

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
