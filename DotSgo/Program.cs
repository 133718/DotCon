using System;
using System.Threading.Tasks;
using DotBot;

namespace DotSgo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => new SgoBot().RunAsync());
            Console.ReadLine();
        }
    }
}
