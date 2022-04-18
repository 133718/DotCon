using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using Serilog;
using System.Text.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using DotBot.Services;
using Serilog.Events;
using SgoApi;
using DotBot.Models;

namespace DotBot
{
    public class SgoBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private JsonConfig _config;
        private SgoClient _sgoClient;

        public bool Connected { get; private set; }

        public async Task RunAsync()
        {
            Connected = true;
            try
            {
                string jsonString = File.ReadAllText("config.json");

                _config = JsonSerializer.Deserialize<JsonConfig>(jsonString);
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception("Config not found", ex);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Config parse error", ex);
            }

            _sgoClient = new SgoClient(_config.Username, _config.Password);
            await _sgoClient.Connection.ConnectAsync();

            _services = BuildServiceProvider();

            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();
            await _services.GetRequiredService<ServerNormolizeService>().InitializeAsync();

            Log.Information("[{Source}] {Message}", "Bot", "Conecting...");

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
        }

        public async Task StopAsync()
        {
            if (Connected)
            {
                Connected = false;
                _services.GetRequiredService<CommandHandlerService>().Stop();

                await _sgoClient.Connection.DisconnectAsync();
                await _client.StopAsync();

                Log.Information("[{Source}] {Message}", "Bot", "Bot stoped");

                _client.Dispose();
                _sgoClient.Dispose();
            }
        }

        private IServiceProvider BuildServiceProvider()
        {
            var _clientConfig = new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug };
            _client = new DiscordSocketClient(_clientConfig);

            var _commandConfig = new CommandServiceConfig() { LogLevel = LogSeverity.Debug };
            _commands = new CommandService(_commandConfig);

            _client.Log += LogAsync;
            _commands.Log += LogAsync;

            _client.Ready += () =>
            {
                Log.Information("[{Source}] {Message}", "Bot", "Client ready");
                return Task.CompletedTask;
            };

            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_config)
                //.AddSingleton(new NotificationService())
                //.AddSingleton<DatabaseService>()
                .AddSingleton<ServerNormolizeService>()
                .AddSingleton<DotUser>()
                .AddSingleton(_sgoClient)
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
