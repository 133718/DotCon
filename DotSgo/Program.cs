using System;
using System.Threading.Tasks;
using DotBot;
using Serilog;

namespace DotSgo
{
    class Program
    {
        static SgoBot bot;

        static void Main(string[] args)
        {
            bot = new SgoBot();

            var result = bot.RunAsync().Result;
            Log.Information(result.ToString());

            if (!result.IsSuccess)
                return;

            while (Console.ReadLine() != "exit") 
            {
                Log.Information("lol");
            }
            Log.Information(bot.StopAsync().Result.ToString());

            //var client = new SgoClient("Проходский", "testtest");
        }
    }
}
