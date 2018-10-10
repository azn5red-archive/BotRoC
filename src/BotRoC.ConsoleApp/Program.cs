using System;
using log4net;

namespace BotRoC.ConsoleApp
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            AdbClass adbClass = new AdbClass();
            if (adbClass.StartServer() == 0)
            {
                log.Info("Test");
                Console.WriteLine(adbClass.GetScreenSize());
                adbClass.TouchScreen(50, 750);
            }
        }
    }
}
