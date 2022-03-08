using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using Serilog;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotBot
{
    public class SgoBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private ServiceProvider _services;
        private JsonConfig _config;

        public SgoBot()
        {

        }

        public async Task<BotResult> Run()
        {

            try
            {
                string jsonString = File.ReadAllText("config.json");

                _config = JsonConvert.DeserializeObject<JsonConfig>(jsonString);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "Config not found");
                return BotResult.Error(ex, "Config not found");
            }
            catch (JsonException ex)
            {
                Log.Error(ex.Message);
                return BotResult.Error(ex, "Invalid config.json");
            }

            BuildServiceProvider();

            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();

            _client.Ready += () =>
            {
                Log.Information("Client ready");
                return Task.CompletedTask;
            };

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            return BotResult.Success("Bot started successfuly");
        }


        private IServiceProvider BuildServiceProvider()
        {
            var _clientConfig = new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug };
            _client = new DiscordSocketClient(_clientConfig);

            var _commandConfig = new CommandServiceConfig() { LogLevel = LogSeverity.Debug };
            _commands = new CommandService(_commandConfig);

            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_config)
                //.AddSingleton(new NotificationService())
                //.AddSingleton<DatabaseService>()
                .AddSingleton<CommandHandlerService>()
                .BuildServiceProvider();
        }
    }
}
