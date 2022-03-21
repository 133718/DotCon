using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;   

namespace DotBot.Services
{
    internal class SgoConnectionService
    {
        private readonly JsonConfig _config;
        private HttpClient sgoClient;
        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler();

        public SgoConnectionService(JsonConfig jsonConfig)
        {
            _config = jsonConfig;
        }
    }
}
