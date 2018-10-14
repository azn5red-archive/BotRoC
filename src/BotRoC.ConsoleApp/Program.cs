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
            AdbClass adbClass = new AdbClass();
            if (adbClass.StartServer() == 0)
            {
                adbClass.GetScreenSize();
                Point point = new Point(50, 700);
                adbClass.TouchPoint(point);

            }
            Bitmap needle = ImageUtil.OpenImage("Screen1.png");
            Bitmap haystack = ImageUtil.OpenImage("screenshot.jpg");
            Rectangle image = ImageUtil.SearchBitmap(needle, haystack, 0.0);
            adbClass.TouchRectangle(image);
        }
    }
}
