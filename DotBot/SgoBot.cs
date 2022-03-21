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
using Serilog.Events;

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

        private IServiceProvider BuildServiceProvider()
        {
            var _clientConfig = new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug };
            _client = new DiscordSocketClient(_clientConfig);

            var _commandConfig = new CommandServiceConfig() { LogLevel = LogSeverity.Debug };
            _commands = new CommandService(_commandConfig);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            _client.Log += LogAsync;
            _commands.Log += LogAsync;

            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_config)
                .AddSingleton<StopService>()
                //.AddSingleton(new LogService(_client, _commands))
                //.AddSingleton(new NotificationService())
                //.AddSingleton<DatabaseService>()
                .AddSingleton<SgoConnectionService>()
                .AddSingleton<CommandHandlerService>()
                .BuildServiceProvider();
        }

        private static async Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Debug => LogEventLevel.Debug,
                _ => LogEventLevel.Information
            };
            Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);
            await Task.CompletedTask;
        }
    }
}
