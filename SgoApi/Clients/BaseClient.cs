using SgoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SgoApi.Clients
{
    public abstract class BaseClient
    {
        readonly User user;

        internal BaseClient(User user) => this.user = user;

        protected async ValueTask<string> SendHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            if (!request.Headers.Contains("User-Agent"))
            {
                request.Headers.Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36"
                );
            }

            using var response = await user.Client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})." +
                    Environment.NewLine +
                    "Request:" +
                    Environment.NewLine +
                    request
                );
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        protected async ValueTask<string> SendHttpRequestAsync(string url, HttpMethod method, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(method, url);
            return await SendHttpRequestAsync(request, cancellationToken);
        }
        
    }
}
