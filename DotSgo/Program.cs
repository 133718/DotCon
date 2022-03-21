using System;
using System.Threading.Tasks;
using DotBot;
using SgoApi;

namespace DotSgo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Task.Run(() => new SgoBot().RunAsync());
            //Console.ReadLine();
            var client = new SgoClient("Проходский", "testtest");
            try
            {
                await client.ConnectAsync();
            }
            finally
            {
                await client.DisconnectAsync();
                client.Dispose();
            }
        }
    }
}
