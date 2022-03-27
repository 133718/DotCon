using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace SgoApi.Models
{
    public class User : IDisposable
    {
        public string Username { get; }
        public string Password { get; }

        public UserLogInfo LogInfo { get; private set; }
        DateTime connectedTime;
        public bool Conected { get; private set; } = false;

        readonly HttpClient client;
        readonly Dictionary<string, string> form = new()
                {
                    { "LoginType", "1" },
                    { "cid", "2" },
                    { "sid", "1" },
                    { "pid", "-1" },
                    { "cn", "1" },
                    { "sft", "2" },
                    { "scid", "1" },
                    { "UN", null },
                    { "PW", null },
                    { "lt", null },
                    { "pw2", null },
                    { "ver", null }
                };

        public HttpClient Client => client;

        public bool IsConected => Conected && connectedTime.AddHours(1) > DateTime.Now;

        internal User(string username, string password, LoginForm loginForm = null)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            client = new(new HttpClientHandler() {CookieContainer = new CookieContainer() });
            client.BaseAddress = new Uri("https://sgo.yanao.ru");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0");
            client.DefaultRequestHeaders.Add("Origin", "https://sgo.yanao.ru");
            if (loginForm != null)
            {
                Console.WriteLine("ACHTUNG 3.14 WAS OBNARUSHIT");
                form["cid"] = loginForm.countryId;
                form["sid"] = loginForm.regionId;
                form["pid"] = loginForm.provinceId;
                form["cn"] = loginForm.cityId;
                form["sft"] = loginForm.sft;
                form["scid"] = loginForm.schoolId;
            }
            Log.Debug("[{Source}] {Message}", "Sgo", $"User {username} have created");
        }

        public void Dispose()
        {
            client?.Dispose();
        }

        public void Connected(UserLogInfo info)
        {
            LogInfo = info ?? throw new ArgumentNullException(nameof(info));
            connectedTime = DateTime.Now;
            Conected = true;
        }

        public void Update()
        {
            connectedTime = DateTime.Now;
        }

        public HttpContent ToContent(Dictionary<string, string> data)
        {
            form["UN"] = Username;
            form["pw2"] = Helper.GetHash(data["salt"] + Helper.GetHash(Password));
            form["PW"] = form["pw2"][Password.Length..];
            form["lt"] = data["lt"];
            form["ver"] = data["ver"];
            return new FormUrlEncodedContent(form);
        }
    }
}
