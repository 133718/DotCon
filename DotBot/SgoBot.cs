using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DotBot
{
    public class SgoBot
    {
		private DiscordSocketClient _client;

		public SgoBot()
        {
			
        }

		public async Task<BotResult> Run()
        {
			JsonConfig jsonConfig;

			try
            {
				string jsonString = File.ReadAllText("config.json");

				jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(jsonString);
			}
			catch (FileNotFoundException ex)
            {
				Console.WriteLine(ex.Message);
				return BotResult.Error(ex, "Config not found");
            }
			catch (JsonException ex)
            {
				Console.WriteLine(ex.Message);
				return BotResult.Error(ex, "Invalid config.json");
            }
			

			var _config = new DiscordSocketConfig { MessageCacheSize = 100, LogLevel = LogSeverity.Debug};
			_client = new DiscordSocketClient(_config);

			await _client.LoginAsync(TokenType.Bot, jsonConfig.Token);
			await _client.StartAsync();

			_client.Ready += () =>
			{
				Console.WriteLine("Client ready");
				return Task.CompletedTask;
			};
			
			return BotResult.Success("Bot started successfuly");
		}
    }
}
