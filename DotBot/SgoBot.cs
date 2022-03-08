using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using Serilog;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotBot.Services;

namespace DotBot
{
    public class SgoBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private JsonConfig _config;

        public async Task RunAsync()
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
            //    .WriteTo.Console()
            //    .CreateLogger();

            

            try
            {
                string jsonString = File.ReadAllText("config.json");

                _config = JsonConvert.DeserializeObject<JsonConfig>(jsonString);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex + "\n Config not found");
                return;
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            _services = BuildServiceProvider();

            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();

            _client.Ready += () =>
            {
                Log.Information("Client ready");
                return Task.CompletedTask;
            };

            Log.Information("Conecting...");

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();

            var token = _services.GetRequiredService<StopService>().GetToken();
            await Task.Delay(-1, token);

            Log.Information("I stoped!!!");
        }

//TODO: исправить логи
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
                .AddSingleton<StopService>()
                .AddSingleton(new LogService(_client, _commands))
                //.AddSingleton(new NotificationService())
                //.AddSingleton<DatabaseService>()
                .AddSingleton<CommandHandlerService>()
                .BuildServiceProvider();
        }
    }
}
