using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using Tesseract;

namespace BotRoC.ConsoleApp
{
    public class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AdbClass adbClass;
        private String resolutionPath;
        // private Point ScoutBase;

        public Bot()
        {
            log.Info("Bot starting...");
            this.adbClass = new AdbClass();

            Point resolution = this.adbClass.GetScreenSize();
            this.resolutionPath = "resources/Game/resolutions/" + resolution.X + 'x' + resolution.Y + '/';
            log.Info("Resource folder used : " + this.resolutionPath);
        }

        public AdbClass GetAdbClass()
        {
            return this.adbClass;
        }

        public void StartGame()
        {
            log.Info("Game launched...");
            this.TouchImage("App.jpg");
            while (1 == 1)
            {
                try
                {
                    Thread.Sleep(5000);
                    Rectangle home1 = FindImage("Home1.jpg");
                    break;
                }
                catch (ImageNotFound)
                {
                    log.Info("Game starting...");
                }
            }
            log.Info("Game started!");
        }

        public void ReadScreen()
        {
            try
            {
                log.Info("OCR read screen");
                var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
                log.Info(engine);
                //var img = Pix.LoadFromFile(@"./test.png");
                var conv = new BitmapToPixConverter();
                var img = conv.Convert(adbClass.GetAdbScreen());
                var page = engine.Process(img);
                log.Info("OCR : " + page.GetText());
                Console.WriteLine(page.GetText());
            }
            catch (Exception e)
            {
                log.Debug(e);
            }
        }

        private Rectangle FindImage(string path)
        {
            log.Debug("Trying to find image on screen  : " + this.resolutionPath + path);
            Bitmap needle = ImageUtil.OpenImage(this.resolutionPath + path);
            Bitmap screen = adbClass.GetAdbScreen();
            Rectangle image = ImageUtil.SearchBitmap(needle, screen, 0.0);
            if (image.Width != 0)
                return image;
            else
                throw new ImageNotFound();
        }

        private void TouchImage(string path)
        {
            Rectangle image = this.FindImage(path);
            adbClass.TouchRectangle(image);
        }

    }


    class ImageNotFound : Exception
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageNotFound()
        {
            log.Debug("Image not found");
        }
    }
}
