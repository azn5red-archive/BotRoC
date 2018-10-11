using System;
using System.Threading;
using System.Drawing;

namespace BotRoC.ConsoleApp
{
    public class ImageUtil
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ImageUtil() { }

        public Bitmap OpenImage(string path)
        {
            return new Bitmap(path);
        }
    }
}
