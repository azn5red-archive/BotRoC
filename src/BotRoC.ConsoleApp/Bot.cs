using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using Tesseract;

namespace BotRoC.ConsoleApp
{
    public class Bot
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AdbClass adbClass;
        private String resolutionPath;

        public Bot()
        {
            log.Info("Bot starting...");
            this.adbClass = new AdbClass();

            Point resolution = this.adbClass.GetScreenSize();
            this.resolutionPath = "resources/Game/resolutions/" + resolution.X + 'x' + resolution.Y + '/';
            if (!Directory.Exists(this.resolutionPath))
            {
                log.Error("Resource folder not found");
                throw new FileNotFoundException();
            }
            else
            {
                log.Debug("Resource folder used : " + this.resolutionPath);
            }
        }

        public AdbClass GetAdbClass()
        {
            return this.adbClass;
        }

        public void StartGame()
        {
            int i = 0;

            log.Info("Trying to start the game...");
            try
            {
                this.TouchImage("App.jpg", 0.0);
                log.Info("Game launched");
            }
            catch
            {
                log.Info("Game already started ?");
            }
            while (1 == 1)
            {
                i++;
                if (i == 10)
                    throw new TimeoutException();
                try
                {
                    Rectangle ListMenu = FindImage("ListMenu.jpg", 0.4);
                    break;
                }
                catch (ImageNotFound)
                {
                    log.Info("Game starting...");
                    Thread.Sleep(5000);
                }
            }
            log.Info("Game started!");
        }

        public void ReadScreen()
        {
            try
            {
                log.Info("OCR read screen");
                TesseractEnviornment.CustomSearchPath = "resources/Tesseract";
                var engine = new TesseractEngine(@"./resources/Tesseract/tessdata", "eng", EngineMode.Default);     //var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
                var conv = new BitmapToPixConverter();
                var img = conv.Convert(ImageUtil.RemoveNoise(ImageUtil.SetGrayscale(adbClass.GetAdbScreen())));
                var page = engine.Process(img);
                log.Info("OCR : " + page.GetText());
            }
            catch (Exception e)
            {
                log.Debug(e);
            }
        }

        public void CollectResources()
        {
            log.Debug("Trying to collect resources...");
            String[] ResourcesFiles =
            {
                "Help.jpg",
                "AskHelp.jpg",
                "Food.jpg",
                "Gold.jpg",
                "Rock.jpg",
                "Wood.jpg",
            };

            foreach (string resource in ResourcesFiles)
            {
                try
                {
                    log.Info("Collecting " + resource.Split('.')[0]);
                    this.TouchImage(resource, 0.4);
                }
                catch
                {
                    log.Info("Can't find " + resource.Split('.')[0]);
                }
            }
            log.Info("Collection terminated");
        }

        public void Explore()
        {
            while (1 == 1)
            {
                try
                {
                    this.TouchImage("ExploreNotif.jpg", 0.5, 0, 200);
                }
                catch
                {
                    return;
                }
                Thread.Sleep(2000);
                this.TouchImage("ExploreMenu.jpg", 0.3);
                Thread.Sleep(2000);
                this.TouchImage("ExploreButton.jpg", 0.2);
                Thread.Sleep(2000);
                this.TouchImage("ExploreButton.jpg", 0.2);
                Thread.Sleep(2000);
                this.TouchImage("SendButton.jpg", 0.2);
                Thread.Sleep(2000);
                this.GoToTown();
                Thread.Sleep(2000);
            }
        }

        public void GoToTown()
        {
            try
            {
                log.Info("Going back to town");
                Rectangle map = this.FindImage("Map.jpg", 0.4);
                log.Info("Already in town !");
            }
            catch (ImageNotFound)
            {
                try
                {
                    this.TouchImage("Home.jpg", 0.4);
                }
                catch
                {
                    log.Error("I'm lost");
                }
            }
        }

        private Rectangle FindImage(string path, Double tolerance)
        {
            log.Debug("Trying to find image on screen  : " + this.resolutionPath + path);
            Bitmap needle = ImageUtil.OpenImage(this.resolutionPath + path);
            Bitmap screen = adbClass.GetAdbScreen();
            Rectangle image = ImageUtil.SearchBitmap(needle, screen, tolerance);
            if (image.Width != 0)
                return image;
            else
                throw new ImageNotFound();
        }

        private void TouchImage(string path, Double tolerance)
        {
            adbClass.TouchRectangle(this.FindImage(path, tolerance));
        }

        private void TouchImage(string path, Double tolerance, int x, int y)
        {
            adbClass.TouchRectangle(this.FindImage(path, tolerance), x, y);
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
