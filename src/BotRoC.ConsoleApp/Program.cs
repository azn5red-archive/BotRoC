using System;
using System.Drawing;
using log4net;

namespace BotRoC.ConsoleApp
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {
            try
            {
                Bot bot = new Bot();
                // bot.StartGame();
                bot.ReadScreen();
            }
            catch
            {
                log.Error("Bot stopped");
            }

        }
    }
}
