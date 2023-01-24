using RestClientApi.UserInterface;
using System.Threading.Tasks;
using System;

namespace RestClientApi
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleRender.Init("FileServer", 30, 100, System.ConsoleColor.DarkRed, ConsoleColor.Black);
            await ConsoleRender.StartAsync();
        }
    }
}

