using System;
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
                bot.GetAdbClass().TakeScreenShot("test.jpg");

                bot.StartGame();
                //bot.ReadScreen();
                while (1 == 1)
                {
                    bot.CollectResources();
                    bot.Explore();
                    bot.CollectTribalVillage();
                }
                
            }
            catch
            {
                log.Error("Bot stopped");
            }

        }
    }
}
