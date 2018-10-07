using System;
using BotRoC.ClassLib;

namespace BotRoC.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AdbServer server = new AdbServer();
            int res = server.StartServer();
            server.GetScreenSize();
        }
    }
}
