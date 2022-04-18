using SgoApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Serilog;

namespace SgoApi.Clients
{
    public class ConnectionClient : BaseClient
    {

        internal ConnectionClient(User user) : base(user) { }

        async ValueTask<Dictionary<string, string>> GetData()
        {
            var response = await SendHttpRequestAsync("webapi/auth/getdata", HttpMethod.Post);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(response);
        }

        public async Task ConnectAsync() 
        {
            Log.Debug("[{Source}] {Message}", "Sgo", "Start connecting to Sgo");
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "webapi/login");
                var salt = await GetData() ?? throw new ArgumentNullException("GetData() returned null");
                request.Content = user.ToContent(salt);
                request.Headers.Add("Referer", "https://sgo.yanao.ru/about.html");
                var response = await SendHttpRequestAsync(request) ?? throw new ArgumentNullException("Incorrect user data");
                var logInfo = JsonSerializer.Deserialize<UserLogInfo>(response);
                user.Connected(logInfo);
                Log.Debug("[{Source}] {Message}", "Sgo", "Connect Successful");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Login error", ex);
            }
            catch(JsonException ex)
            {
                throw new HttpRequestException("Request parse error", ex);
            }
        }

        public async Task DisconnectAsync()
        {
            Log.Debug("[{Source}] {Message}", "Sgo", "Starting Disconnect");
            if (!user.IsConected)
                    return;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "asp/logout.asp");
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "at", user.LogInfo.AuthToken } });
                await SendHttpRequestAsync(request);
            }
            catch(HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Found)
            {
                
            }
            catch(Exception ex)
            {
                throw new Exception("LogOut error", ex);
            }
            Log.Debug("[{Source}] {Message}", "Sgo", "Disconnect Successful");
        }
    }

    
}
