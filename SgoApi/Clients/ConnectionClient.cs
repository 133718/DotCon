using SgoApi.Models;
using SgoApi.Results;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SgoApi.Clients
{
    internal class ConnectionClient : BaseClient
    {
        readonly User user;

        public ConnectionClient(User user) : base(user)
        {
            this.user = user;
        }

        async ValueTask<Dictionary<string, string>> GetData()
        {
            var response = await SendHttpRequestAsync("webapi/auth/getdata", HttpMethod.Post);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
        }

        public async Task<RuntimeResult> ConnectAsync() 
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "webapi/login");
                var salt = await GetData() ?? throw new ArgumentNullException("GetData() returned null");
                request.Content = user.ToContent(salt);
                request.Headers.Add("Referer", "https://sgo.yanao.ru/about.html");
                var logInfo = JsonConvert.DeserializeObject<UserLogInfo>(await SendHttpRequestAsync(request) ?? throw new ArgumentNullException("Incorrect user data"));
                user.Connected(logInfo);
                return RuntimeResult.FromSuccess();
            }
            catch (HttpRequestException ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return RuntimeResult.FromError(ex);
            }
            catch(JsonException ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return RuntimeResult.FromError(ex);
            }
        }

        public async Task<RuntimeResult> DisconnectAsync()
        {
            if(!user.IsConected)
                    return RuntimeResult.FromSuccess();
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "asp/logout.asp");
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "at", user.LogInfo.authToken } });
                await user.Client.SendAsync(request);
            }
            catch(HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Found)
            {
                return RuntimeResult.FromSuccess();
            }
            return RuntimeResult.FromSuccess();
        }
    }

    
}
