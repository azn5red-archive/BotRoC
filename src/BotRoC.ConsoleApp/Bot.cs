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
        private Point resolution;
        private String resolutionPath;

        public Bot()
        {
            log.Info("Bot starting...");
            this.adbClass = new AdbClass();

            this.resolution = this.adbClass.GetScreenSize();
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
                    Rectangle EventQuest = FindImage("EventQuest.jpg", 0.4);
                    break;
                }
                catch (ImageNotFound)
                {
                    log.Info("Game starting...");
                    Thread.Sleep(5000);
                }
            }
            log.Info("Game started!");
            //this.GoToTown();
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
                    log.Info("Trying to explore...");
                    this.TouchImage("ExploreNotif.jpg", 0.5, 0, 200);
                }
                catch
                {
                    log.Info("No exploration to do");
                    return;
                }
                try
                {
                    log.Info("Starting new exploration");
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreMenu.jpg", 0.3);
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreButton.jpg", 0.2);
                    Thread.Sleep(3000);
                    this.TouchImage("ExploreButton.jpg", 0.2);
                    Thread.Sleep(3000);
                    this.TouchImage("SendButton.jpg", 0.2);
                    Thread.Sleep(3000);
                    this.GoToTown();
                    Thread.Sleep(3000);
                }
                catch
                {
                    log.Info("Error during exploration");
                    this.GoToTown();
                }
            }
        }

        public void GoToTown()
        {
            int numError = 0;
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
                    //this.TouchImage("Home.jpg", 0.4);
                    adbClass.TouchPoint(new Point(90, 1100));
                }
                catch
                {
                    log.Error("I'm lost");
                    if (numError++ < 5)
                        this.GoToTown();
                    else
                        throw new ImageNotFound();
                }
            }
        }

        public void CollectTribalVillage()
        {
            try
            {
                while (1 == 1)
                {
                    log.Info("Trying to collect gift from tribal villages...");
                    this.GoToMessages();
                    Thread.Sleep(2000);
                    this.FindImage("ExplorationReportSelected.jpg", 0.4);
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationTribalVillage.jpg", 0.3, 90, 0);
                    Thread.Sleep(5000);
                    adbClass.TouchPoint(new Point(this.resolution.X / 2, this.resolution.Y / 2));
                    Thread.Sleep(2000);
                    this.GoToTown();
                    Thread.Sleep(2000);
                }
            }
            catch
            {
                try
                {
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationReportNotSelected.jpg", 0.3);
                    Thread.Sleep(2000);
                    this.TouchImage("ExplorationTribalVillage.jpg", 0.3, 90, 0);
                    Thread.Sleep(5000);
                    adbClass.TouchPoint(new Point(this.resolution.X / 2, this.resolution.Y / 2));
                    Thread.Sleep(2000);
                    this.GoToTown();
                    Thread.Sleep(2000);
                }
                catch
                {
                    log.Info("Nothing to collect");
                    adbClass.TouchPoint(new Point(1850, 75));
                    return;
                }
            }
        }

        public void GoToMessages()
        {
            adbClass.TouchPoint(new Point(1800, 950));
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
